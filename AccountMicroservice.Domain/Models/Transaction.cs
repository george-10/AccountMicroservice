using System;
using System.Collections.Generic;

namespace AccountMicroservice.Domain.Models;

public partial class Transaction
{
    public long? AccountId { get; set; }

    public long? Amount { get; set; }

    public bool? Deposit { get; set; }

    public long Id { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? BranchId { get; set; }

    public virtual Account? Account { get; set; }
}
