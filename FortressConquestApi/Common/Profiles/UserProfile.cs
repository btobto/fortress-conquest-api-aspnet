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
            CreateMap<User, UserItemDTO>();
            CreateMap<CreateUserDTO, User>();

            CreateMap<PaginatedList<User>, PaginatedList<UserItemDTO>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
