using MiniBankingSystem.Application;
using MiniBankingSystem.Domain.ValueObjects;
using MiniBankingSystem.Infrastructure.Repositories;

Console.WriteLine("Hello, World!");

//var hundredUSD = Money.Of(100, "USD");
//var fifty = Money.Of(50, "USD");

//var addResult = hundredUSD.Add(fifty);
//var subtractResult = hundredUSD.Subtract(Money.Of(50, "USD"));

//Console.WriteLine("Add Result => " + addResult.Amount + addResult.Currency);
//Console.WriteLine("Subtract Result => " + subtractResult.Amount + subtractResult.Currency);

var repository = new InMemoryAccountRepository();
var service = new AccountService(repository);

// Create Account
var account = service.OpenAccount("Kyats");
var accountId = account.Id;
Console.WriteLine(accountId.Value);

// Deposit
service.Deposit(accountId, 500000, "Kyats");
Console.WriteLine(account.Balance);

// Withdraw
service.Withdraw(accountId, 100000, "Kyats");
Console.WriteLine(account.Balance);
