using MiniBankingSystem.Domain.ValueObjects;

Console.WriteLine("Hello, World!");

var hundredUSD = Money.Of(100, "USD");
var fifty = Money.Of(50, "USD");

var addResult = hundredUSD.Add(fifty);
var subtractResult = hundredUSD.Subtract(Money.Of(300, "USD"));

Console.WriteLine("Add Result => " + addResult.Amount + addResult.Currency);
Console.WriteLine("Subtract Result => " + subtractResult.Amount + subtractResult.Currency);