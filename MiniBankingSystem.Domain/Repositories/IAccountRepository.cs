using MiniBankingSystem.Domain.Entities;
using MiniBankingSystem.Domain.ValueObjects;

namespace MiniBankingSystem.Domain.Repositories;

public interface IAccountRepository
{
    Account? GetById(AccountId id);
    void Save(Account account);
}