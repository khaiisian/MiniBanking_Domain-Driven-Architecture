using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBankingSystem.Application;
using MiniBankingSystem.Domain.ValueObjects;

namespace MiniBankingSystem.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("createAccount")]
    public IActionResult CreateAccount(string currency)
    {
        var account = _accountService.OpenAccount(currency);
        return Ok(account);
    }

    //[HttpGet("getAccountById")]
    //public IActionResult CreateAccount(string currency)
    //{
    //    var account = _accountService.OpenAccount(currency);
    //    return Ok(account);
    //}

    [HttpPost("Deposit")]
    public IActionResult Deposit(Guid id, DepositRequest req)
    {
        _accountService.Deposit(new AccountId(id), req.Amount!.Value, req.Currency!);
        
        return Ok();
    }

    [HttpPost("Withdraw")]
    public IActionResult Withdraw(Guid id, DepositRequest req)
    {
        _accountService.Withdraw(new AccountId(id), req.Amount!.Value, req.Currency!);
        return Ok();
    }
}

public class DepositRequest
{
    public decimal ? Amount { get; set; }
    public string ? Currency { get; set; }
}

public class WithdrawRequest
{
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
}
