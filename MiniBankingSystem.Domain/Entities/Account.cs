using MiniBankingSystem.Domain.ValueObjects;

namespace MiniBankingSystem.Domain.Entities;

public class Account
{
    public AccountId Id { get; }
    public Money Balance { get; private set;  }

    private Account (AccountId id, Money balance)
    {
        Id = id;
        Balance = balance;
    }

    // Open Account
    public static Account Open(string currency)
    {
        return new Account(AccountId.New(), Money.Of(0, currency));
    }

    // Deposit
    public void Deposit (Money amount)
    {
        if(amount.Amount <= 0)
        {
            throw new DomainException("Deposit amount must be positive.");
        }

        Balance = Balance.Add(amount);
    }

    // Withdraw
    public void Withdraw (Money amount)
    {
        if (amount.Amount <= 0)
        {
            throw new DomainException("Withdraw amount must be positive.");
        }

        if(amount.Amount > Balance.Amount)
        {
            throw new DomainException("Insufficient funds.");
        }

        Balance = Balance.Subtract(amount);
    }
}
