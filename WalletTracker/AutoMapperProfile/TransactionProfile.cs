using AutoMapper;
using WalletTracker.Dtos;
using WalletTracker.Models;

namespace WalletTracker.AutoMapperProfile;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<TransactionUpdateDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => 
                srcMember != null));
    }
}