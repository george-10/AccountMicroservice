using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountMicroservice.Domain.Models;

public partial class Transaction
{
    [Required]
    public long? AccountId { get; set; }
    [Required]
    public long? Amount { get; set; }
    [Required]
    public bool? Deposit { get; set; }
    [Required]
    public long Id { get; set; }
    [Required]
    public DateTime? Timestamp { get; set; }
    [Required]
    public string? BranchId { get; set; }

    public virtual Account? Account { get; set; }
}
