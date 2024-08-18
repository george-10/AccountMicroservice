using AccountMicroservice.Application.Services.RecurrentTransactionService.ViewModel;
using AccountMicroservice.Domain.Models;
using AutoMapper;

namespace AccountMicroservice.Application.Mappers;

public class AccountProfile :Profile
{
    public AccountProfile()
    {
        CreateMap<AccountViewModel, Account>()
            .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Balance, opt => opt.MapFrom(src => src.Balance))
            .ForMember(dst =>dst.BranchId,opt => opt.MapFrom(src => src.BranchId));
        
        CreateMap<Account, Accountres>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId )) 
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance )) 
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name )) 
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId )); 

    }
}