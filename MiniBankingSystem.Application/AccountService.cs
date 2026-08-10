using MiniBankingSystem.Domain;
using MiniBankingSystem.Domain.Entities;
using MiniBankingSystem.Domain.Repositories;
using MiniBankingSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBankingSystem.Application;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Account OpenAccount(string currency)
    {
        var account = Account.Open(currency);
        _accountRepository.Save(account);
        return account;
    }

    public void Deposit(AccountId id, decimal amount, string currency)
    {
        var account = _accountRepository.GetById(id);

        if(account is null)
        {
            throw new DomainException("Account Not Found.");
        }

        var money = Money.Of(amount, currency);
        account.Deposit(money);

        _accountRepository.Save(account);
    }

    public void Withdraw(AccountId id, decimal amount, string currency)
    {
        var account = _accountRepository.GetById(id);

        if (account is null)
        {
            throw new DomainException("Account Not Found.");
        }

        var money = Money.Of(amount, currency);
        account.Withdraw(money);

        _accountRepository.Save(account);
    }
}
