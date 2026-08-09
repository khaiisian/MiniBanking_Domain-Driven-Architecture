using MiniBankingSystem.Domain.Entities;
using MiniBankingSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBankingSystem.Infrastructure.Repositories;

public class InMemoryAccountRepository
{
    private readonly Dictionary<AccountId, Account> _accounts = new();

    public void Save (Account account)
    {
        _accounts[account.Id] = account;
    }

    public Account? GetById(AccountId id)
    {
        _accounts.TryGetValue(id, out Account account);
        return account;
    }
}
