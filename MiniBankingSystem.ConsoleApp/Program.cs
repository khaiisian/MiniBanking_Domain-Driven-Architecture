using MiniBankingSystem.Application;
using MiniBankingSystem.Domain.Events;
using MiniBankingSystem.Domain.ValueObjects;
using MiniBankingSystem.Infrastructure.Repositories;
using System.Buffers;

Console.WriteLine("Hello, World!");

//var hundredUSD = Money.Of(100, "USD");
//var fifty = Money.Of(50, "USD");

//var addResult = hundredUSD.Add(fifty);
//var subtractResult = hundredUSD.Subtract(Money.Of(50, "USD"));

//Console.WriteLine("Add Result => " + addResult.Amount + addResult.Currency);
//Console.WriteLine("Subtract Result => " + subtractResult.Amount + subtractResult.Currency);

//var repository = new InMemoryAccountRepository();
//var service = new AccountService(repository);

//// Create Account
//var account = service.OpenAccount("Kyats");
//var accountId = account.Id;
//Console.WriteLine(accountId.Value);

//// Deposit
//service.Deposit(accountId, 500000, "Kyats");
//Console.WriteLine(account.Balance);

//// Withdraw
//service.Withdraw(accountId, 100000, "Kyats");
//Console.WriteLine(account.Balance);

var repository = new InMemoryAccountRepository();
var service = new AccountService(repository);

var account = service.OpenAccount("Ks");
var accountId = account.Id;

service.Deposit(accountId, 500000, "Ks");
service.Withdraw(accountId, 100000, "Ks");

Console.WriteLine("Account Data=======");
Console.WriteLine("Account Id: "+ accountId);
Console.WriteLine("Balance: "+ account.Balance);

Console.WriteLine("=== Reacting to Domain Events ===");
foreach (var domainEvent in account!.DomainEvents)
{
    switch (domainEvent)
    {
        case AccountOpened e:
            Console.WriteLine($"✅ Account {e.AccountId.Value} was opened.");
            break;

        case MoneyDeposited e:
            Console.WriteLine($"📧 Email: You deposited {e.Amount.Amount} {e.Amount.Currency}.");
            break;

        case MoneyWithdrawn e:
            Console.WriteLine($"📧 Email: You withdrew {e.Amount.Amount} {e.Amount.Currency}.");
            break;
    }
}
