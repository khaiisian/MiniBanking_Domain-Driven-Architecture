using System.Runtime.Intrinsics.X86;

namespace MiniBankingSystem.Domain.ValueObjects;

// Value Object (interchangeable and immutable object)
// use record since a class does NOT compare by value by default (records give value equality for free)

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Of(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot empty.");
        }

        return new Money(amount, currency);
    }

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
        {
            throw new InvalidOperationException("Cannot add different currencies.");
        }

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (other.Currency != Currency)
        {
            throw new InvalidOperationException("Cannot subtract different currencies.");
        }

        return Money.Of(Amount - other.Amount, Currency);
    }
}

