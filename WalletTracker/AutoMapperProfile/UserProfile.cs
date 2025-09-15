using AutoMapper;
using WalletTracker.Dtos;
using WalletTracker.Models;

namespace WalletTracker.AutoMapperProfile;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, CreateUserDto>();
        CreateMap<User, UpdateUserDto>();
    }
}