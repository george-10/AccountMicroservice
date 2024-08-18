using AccountMicroservice.Domain.Models;
using AutoMapper;
using Grpc.Core;

namespace AccountMicroservice.Application.Services.gRPC;

public class gRPCTransactionImpl :TransactionService.TransactionServiceBase
{
    private readonly AccountDbContext _context;
    private readonly IMapper _mapper;
    public gRPCTransactionImpl(AccountDbContext context,IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public override Task<GetTransactionsResponse> GetTransactions(GetTransactionsRequest request, ServerCallContext context)
    {
        var res = _context.Transactions.Where(x => x.BranchId == request.BranchId && x.AccountId == request.AccountId).ToList();
        var response = new GetTransactionsResponse();
        foreach (var t in res)
        {
            response.Transactions.Add(_mapper.Map<TransactionRes>(t));
        }

        return Task.FromResult(response);

    }
}