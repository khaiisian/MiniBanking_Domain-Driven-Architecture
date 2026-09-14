using Microsoft.EntityFrameworkCore;
using MiniBankingSystem.Application;
using MiniBankingSystem.Domain.Repositories;
using MiniBankingSystem.Infrastructure.Context;
using MiniBankingSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// no db method
//builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();

builder.Services.AddDbContext<BankingDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("BankingDb"))
);

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<AccountService>();

var app = builder.Build();

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
