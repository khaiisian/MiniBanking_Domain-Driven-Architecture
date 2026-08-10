using MiniBankingSystem.Domain.Entities;
using MiniBankingSystem.Domain.Repositories;
using MiniBankingSystem.Domain.ValueObjects;

namespace MiniBankingSystem.Infrastructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<AccountId, Account> _accounts = new();

    public void Save(Account account)
    {
        _accounts[account.Id] = account;
    }

    public Account? GetById(AccountId id)
    {
        _accounts.TryGetValue(id, out Account? account);
        return account;
    }
}
