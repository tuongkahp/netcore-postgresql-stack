using AutoMapper;
using Datas.Entities;
using Dtos.Auth;
using Dtos.Users;

namespace Apis.AutoMappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, CreateUserReqDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Group, GroupDto>().ReverseMap();
    }
}