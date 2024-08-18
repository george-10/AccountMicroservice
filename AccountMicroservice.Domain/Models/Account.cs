using System;
using System.Collections.Generic;

namespace AccountMicroservice.Domain.Models;

public partial class Account
{
    public long? Balance { get; set; }

    public long? UserId { get; set; }

    public long Id { get; set; }

    public string? Name { get; set; }

    public string? BranchId { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
