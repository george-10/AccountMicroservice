namespace AccountMicroservice.Application.Services.RecurrentTransactionService.ViewModel;

public class TransactionViewModel
{
    public long Id { get; set; }
    public long? AccountId { get; set; }
    public bool? IsDeposit { get; set; }
    public long? Amount { get; set; }
    public long? Interval { get; set; } // Interval in seconds
    public string? BranchId { get; set; }
    public DateTime? LastTransactionDate { get; set; }

}