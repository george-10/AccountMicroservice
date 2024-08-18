using AccountMicroservice.Domain.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace AccountMicroservice.Application.Services.gRPC;

public class gRPCRollbackService :AdminRollBackService.AdminRollBackServiceBase
{
    private readonly AccountDbContext _context;

    public gRPCRollbackService(AccountDbContext context)
    {
        _context = context;
    }
    public override async Task<RollbackResponse> Rollback(RollbackRequest request, ServerCallContext context)
    {
        IQueryable<Transaction> transactionsQuery;

      
        if (request.AllBranches)
        {
            transactionsQuery = _context.Transactions.Where(t => t.Timestamp >= Convert.ToDateTime(request.Date));
        }
        else
        {
            transactionsQuery = _context.Transactions.Where(t => t.Timestamp >= Convert.ToDateTime(request.Date) && t.BranchId == request.BranchId);
        }


        var transactions = await transactionsQuery.ToListAsync();

        foreach (var transaction in transactions)
        {
            var accountId = transaction.AccountId;
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == accountId);

            if (account != null)
            {
                account.Balance = (bool)transaction.Deposit 
                    ? account.Balance -= transaction.Amount 
                    : account.Balance += transaction.Amount;

                await _context.SaveChangesAsync();
            }
        }

        _context.Transactions.RemoveRange(transactions);
        await _context.SaveChangesAsync();

        var response = new RollbackResponse
        {
            Status = true
        };
    
        return await Task.FromResult(response);
    }

}