using AutoMapper;
using FortressConquestApi.DTOs;
using FortressConquestApi.Models;

namespace FortressConquestApi.Common.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();
        }
    }
}
