using Microsoft.EntityFrameworkCore;
using MiniBankingSystem.Domain.Entities;
using MiniBankingSystem.Domain.ValueObjects;

namespace MiniBankingSystem.Infrastructure.Context;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
    {
    }

    public BankingDbContext()
    {
    }

    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var account = modelBuilder.Entity<Account>();

        account.ToTable("Accounts");

        account.Ignore(a => a.DomainEvents);

        account.HasKey(a => a.Id);
        account.Property(a => a.Id)
               .HasConversion(
                    id => id.Value,                 
                    value => new AccountId(value))  
               .ValueGeneratedNever();              

        account.OwnsOne(a => a.Balance, balance =>
        {
            balance.Property(m => m.Amount)
                   .HasColumnName("Balance_Amount")
                   .HasPrecision(18, 2);

            balance.Property(m => m.Currency)
                   .HasColumnName("Balance_Currency")
                   .HasMaxLength(10);
        });
    }
}
