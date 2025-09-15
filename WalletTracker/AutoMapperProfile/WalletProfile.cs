using AutoMapper;
using WalletTracker.Dtos;
using WalletTracker.Models;

namespace WalletTracker.AutoMapperProfile;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<Wallet, WalletDto>();
        CreateMap<WalletDto, Wallet>();
    }
}