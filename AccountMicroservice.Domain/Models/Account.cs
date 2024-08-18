using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountMicroservice.Domain.Models;

public partial class Account
{
    [Required]
    public long? Balance { get; set; }
    [Required]
    public long? UserId { get; set; }
    [Required]
    public long Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? BranchId { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
