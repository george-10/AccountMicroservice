using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AccountMicroservice.Domain.Models;

public partial class AccountDbContext : DbContext
{
    public AccountDbContext()
    {
    }

    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5433;Database=Account_db;Username=username;Password=mysecretpassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accounts_pk");

            entity.Property(e => e.BranchId).HasColumnType("character varying");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.UserId).HasColumnName("User_id");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_pk");

            entity.ToTable("transaction");

            entity.Property(e => e.AccountId).HasColumnName("Account_id");
            entity.Property(e => e.BranchId).HasColumnType("character varying");
            entity.Property(e => e.Timestamp).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("transaction_accounts_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
