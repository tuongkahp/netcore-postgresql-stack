using AutoMapper;
using Datas.Entities;
using Dtos.Auth;
using Dtos.Users;

namespace Apis.AutoMappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RegisterUserDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}