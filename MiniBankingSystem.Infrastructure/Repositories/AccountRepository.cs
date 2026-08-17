using MiniBankingSystem.Domain.Entities;
using MiniBankingSystem.Domain.Repositories;
using MiniBankingSystem.Domain.ValueObjects;
using MiniBankingSystem.Infrastructure.Context;

namespace MiniBankingSystem.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository        
{
    private readonly BankingDbContext _dbContext;

    public AccountRepository(BankingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Account? GetById(AccountId id)
    {
        return _dbContext.Accounts.FirstOrDefault(a => a.Id == id);
    }

    // 2b. save an account
    public void Save(Account account)
    {
        bool exists = _dbContext.Accounts.Any(a => a.Id == account.Id);

        if (!exists)
        {
            _dbContext.Accounts.Add(account);   
        }

        _dbContext.SaveChanges();             
    }
}