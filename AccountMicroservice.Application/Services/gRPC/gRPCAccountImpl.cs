using AccountMicroservice.Domain.Models;
using AutoMapper;
using Grpc.Core;

namespace AccountMicroservice.Application.Services.gRPC;

public class gRPCAccountImpl:CustomerAccountService.CustomerAccountServiceBase
{
    private readonly AccountDbContext _context;
    private readonly IMapper _mapper;
    public gRPCAccountImpl(AccountDbContext context,IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public override Task<GetAccountResponse> GetAccount(GetAccountRequest request, ServerCallContext context)
    {
        var res = _context.Accounts.Where(x => x.UserId == request.UserId).ToList();
        var response = new GetAccountResponse();
        foreach (var t in res)
        {
            response.Account.Add(_mapper.Map<Accountres>(t));
        }

        return Task.FromResult(response);
    }
}