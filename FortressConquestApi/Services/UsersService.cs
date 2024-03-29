﻿using AutoMapper;
using FortressConquestApi.Common;
using FortressConquestApi.Common.Exceptions;
using FortressConquestApi.Data;
using FortressConquestApi.DTOs;
using FortressConquestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FortressConquestApi.Services
{
    public class UsersService
    {
        private readonly FortressConquestContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersService> _logger;

        public UsersService(
            FortressConquestContext context,
            IMapper mapper,
            ILogger<UsersService> logger) 
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedList<User>> GetUsersSortedByXP(int page, int pageSize)
        {
            return await PaginatedList<User>.CreateAsync(
                _context.Users.OrderByDescending(u => u.XP).AsNoTracking(), page, pageSize);
        }

        public async Task<User?> GetUser(Guid id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Character)
                .Include(u => u.FortressesOwned)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                user.FortressCount = user.FortressesOwned.Count;
            }

            return user;
        }

        public async Task<User?> CreateUser(CreateUserDTO userDto)
        {
            var emailTaken = await IsEmailTaken(userDto.Email);

            if (emailTaken)
            {
                throw new EmailTakenException("A user with this email address already exists.");
            }

            var characterDto = CharacterClassesService.GetCharacterClass(userDto.CharacterName);

            if (characterDto == null)
            {
                throw new ItemNotFoundException("Invalid character name. Character not found.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            User? user = null;

            try
            {
                var character = _mapper.Map<Character>(characterDto);
                await _context.Characters.AddAsync(character);

                user = _mapper.Map<User>(userDto);
                user.Character = character;
                await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }

            return user;
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new ItemNotFoundException("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task OnBattleWin(BattleResultDTO result)
        {
            var winner = await _context.Users.FindAsync(result.WinnerId);

            if (winner == null)
            {
                throw new ItemNotFoundException("Winner not found.");
            }

            var loser = await _context.Users.FindAsync(result.LoserId);

            if (loser == null)
            {
                throw new ItemNotFoundException("Loser not found.");
            }

            var fortress = await _context.Fortresses.FindAsync(result.FortressId);

            if (fortress == null)
            {
                throw new ItemNotFoundException("Fortress not found.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newXp = winner.XP + XPGained(winner, loser);
                var newLevel = newXp >= XPForNextLevel(winner.Level) ? winner.Level + 1 : winner.Level;

                winner.XP = newXp;
                winner.Level = newLevel;

                if (fortress.Owner.Id != winner.Id)
                {
                    fortress.Owner.Id = winner.Id;
                    fortress.OwnedSince = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        private int XPGained(User winner, User loser)
        {
            var modifier = loser.Level - winner.Level;
            return modifier > 0 
                ? Constants.BaseXPGain + modifier * Constants.BaseXPModifier : Constants.BaseXPGain;
        }

        private int XPForNextLevel(int currentLevel)
        {
            return (int)Math.Floor(Constants.BaseXP * Math.Pow(currentLevel, Constants.LevelModifier));
        }

        private Task<bool> IsEmailTaken(string email)
        {
            return _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
