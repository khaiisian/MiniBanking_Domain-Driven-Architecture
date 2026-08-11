using MiniBankingSystem.Domain.ValueObjects;

namespace MiniBankingSystem.Domain.Events;

public record AccountOpened(AccountId AccountId) : IDomainEvent;

public record MoneyDeposited(AccountId AccountId, Money Amount) : IDomainEvent;

public record MoneyWithdrawn(AccountId AccountId, Money Amount): IDomainEvent;