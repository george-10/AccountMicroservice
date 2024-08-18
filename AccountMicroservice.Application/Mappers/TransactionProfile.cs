using AccountMicroservice.Application.Services.RecurrentTransactionService.ViewModel;
using AccountMicroservice.Domain.Models;
using AutoMapper;

namespace AccountMicroservice.Application.Mappers;

public class TransactionProfile :Profile
{
    public TransactionProfile()
    {
        CreateMap<TransactionViewModel, Transaction>()
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Deposit, opt => opt.MapFrom(src => src.IsDeposit))
            .ForMember(dest => dest.BranchId,opt => opt.MapFrom(src =>src.BranchId));

        CreateMap<Transaction, TransactionRes>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId ?? 0)) // Assuming AccountId is non-nullable in the proto
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount ?? 0))       // Assuming Amount is non-nullable in the proto
            .ForMember(dest => dest.Deposit, opt => opt.MapFrom(src => src.Deposit ?? false)) // Assuming Deposit is non-nullable in the proto
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src =>src.Timestamp.ToString()))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId));

    }
}