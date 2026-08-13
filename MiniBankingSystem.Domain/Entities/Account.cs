using MiniBankingSystem.Domain.Events;
using MiniBankingSystem.Domain.ValueObjects;

namespace MiniBankingSystem.Domain.Entities;

public class Account
{
    public AccountId Id { get; }
    public Money Balance { get; private set;  }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    // == why use AccountId Id ==
    //Guid customerId = ...;
    //Guid accountId = ...;
    //the correct Transfer should be Transfer(accountId, customerId )
    //but with with Guid
    //Transfer(customerId, accountId) will also work with not issues

    private Account() { }

    private Account (AccountId id, Money balance)
    {
        Id = id;
        Balance = balance;
    }


    // Open Account
    public static Account Open(string currency)
    {
        var account = new Account(AccountId.New(), Money.Of(0, currency));
        account.Raise(new AccountOpened(account.Id));
        return account;
    }

    // Deposit
    public void Deposit (Money amount)
    {
        if(amount.Amount <= 0)
        {
            throw new DomainException("Deposit amount must be positive.");
        }
        Balance = Balance.Add(amount);
        Raise(new MoneyDeposited(Id, amount));
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
        Raise(new MoneyWithdrawn(Id, amount));
    }

    // to record an event
    private void Raise (IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
