using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MiniBankingSystem.Infrastructure.Context;

public class BankingDbContextFactory : IDesignTimeDbContextFactory<BankingDbContext>
{
    public BankingDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            "Server=OTGEXPERTBOOKB5\\SQLEXPRESS;Database=MiniBankingDb;User Id=sa;Password=123;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<BankingDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new BankingDbContext(optionsBuilder.Options);
    }
}
