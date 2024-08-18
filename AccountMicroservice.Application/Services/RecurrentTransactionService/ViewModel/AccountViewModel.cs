namespace AccountMicroservice.Application.Services.RecurrentTransactionService.ViewModel;

public class AccountViewModel
{

    public long? Balance { get; set; }

    public long? UserId { get; set; }

    public long Id { get; set; }

    public string? Name { get; set; }
    public string? BranchId { get; set; }
}