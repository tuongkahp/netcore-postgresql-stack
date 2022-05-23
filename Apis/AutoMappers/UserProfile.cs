using AutoMapper;
using Datas.Entities;
using Dtos.Users;

namespace Apis.AutoMappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, RegisterUserDto>().ReverseMap();
    }
}