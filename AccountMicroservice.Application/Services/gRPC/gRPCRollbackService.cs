using AccountMicroservice.Domain.Models;
using Grpc.Core;

namespace AccountMicroservice.Application.Services.gRPC;

public class gRPCRollbackService :AdminRollBackService.AdminRollBackServiceBase
{
    private readonly AccountDbContext _context;

    public gRPCRollbackService(AccountDbContext context)
    {
        _context = context;
    }
    public override Task<RollbackResponse> Rollback(RollbackRequest request, ServerCallContext context)
    {
        IQueryable<Transaction> transactions;
        if (request.AllBranches == true)
        {
            transactions = _context.Transactions.Where(t => t.Timestamp >= Convert.ToDateTime(request.Date));
        }
        else
        {
            transactions = _context.Transactions.Where(t => t.Timestamp >= Convert.ToDateTime(request.Date) && t.BranchId == request.BranchId);
        }

        foreach (var VARIABLE in transactions)
        {
            var accountId = VARIABLE.AccountId;
            var account = _context.Accounts.FirstOrDefault(x => x.UserId == accountId);
            account.Balance = (bool) VARIABLE.Deposit ? account.Balance -= VARIABLE.Amount : account.Balance += VARIABLE.Amount;
        }
        var response = new RollbackResponse
        {
            Status = true
        };
        return Task.FromResult(response);
    }
}