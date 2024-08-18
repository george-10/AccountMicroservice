using AccountMicroservice.Domain.Models;

namespace AccountMicroservice.Application.Services.gRPC;

using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

public class AccountServiceImpl : AccountService.AccountServiceBase
{
    private readonly AccountDbContext _context;

    public AccountServiceImpl(AccountDbContext context)
    {
        _context = context;
    }

    public override async Task<CheckAccountResponse> CheckAccount(CheckAccountRequest request, ServerCallContext context)
    {
        Console.WriteLine("============================="+request.AccountId);
        Console.WriteLine("============================="+request.BranchId);
        var accountExists = await _context.Accounts.Where(x => x.BranchId == request.BranchId)
            .AnyAsync(a => a.Id == request.AccountId);
    Console.WriteLine(accountExists);
        return new CheckAccountResponse
        {
            Exists = accountExists
        };
    }
}
