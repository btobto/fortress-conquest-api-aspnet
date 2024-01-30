using AutoMapper;
using FortressConquestApi.DTOs;
using FortressConquestApi.Models;

namespace FortressConquestApi.Common.Profiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterDTO>()
                .ReverseMap();
        }
    }
}
