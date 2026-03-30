using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts.Operations;
using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.ObjectOrientedProgramming.Lab5.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public IActionResult Create(
        [FromHeader(Name = "X-Session-Key")] System.Guid sessionKey,
        [FromBody] CreateAccountRequest request)
    {
        CreateAccount.Result result = _accountService.CreateAccount(
            sessionKey,
            new CreateAccount.Request(request.AccountNumber, request.PinCode));

        return result switch
        {
            CreateAccount.Result.Success success => Ok(new { success.AccountId }),
            CreateAccount.Result.AccountAlreadyExists => Conflict(new { Message = "Account already exists" }),
            CreateAccount.Result.Unauthorized => Unauthorized(new { Message = "Unauthorized" }),
            _ => BadRequest(),
        };
    }

    [HttpGet("balance")]
    public IActionResult Balance([FromHeader(Name = "X-Session-Key")] System.Guid sessionKey)
    {
        GetBalance.Result result = _accountService.GetBalance(sessionKey);

        return result switch
        {
            GetBalance.Result.Success success => Ok(new { success.Balance }),
            GetBalance.Result.AccountNotFound => NotFound(new { Message = "Account not found" }),
            GetBalance.Result.Unauthorized => Unauthorized(new { Message = "Unauthorized" }),
            _ => BadRequest(),
        };
    }

    [HttpPost("withdraw")]
    public IActionResult WithdrawMoney(
        [FromHeader(Name = "X-Session-Key")] System.Guid sessionKey,
        [FromBody] AmountRequest request)
    {
        Withdraw.Result result = _accountService.Withdraw(sessionKey, new Withdraw.Request(request.Amount));

        return result switch
        {
            Withdraw.Result.Success success => Ok(new { success.NewBalance }),
            Withdraw.Result.InsufficientFunds => BadRequest(new { Message = "Insufficient funds" }),
            Withdraw.Result.InvalidAmount => BadRequest(new { Message = "Invalid amount" }),
            Withdraw.Result.AccountNotFound => NotFound(new { Message = "Account not found" }),
            Withdraw.Result.Unauthorized => Unauthorized(new { Message = "Unauthorized" }),
            _ => BadRequest(),
        };
    }

    [HttpPost("deposit")]
    public IActionResult DepositMoney(
        [FromHeader(Name = "X-Session-Key")] System.Guid sessionKey,
        [FromBody] AmountRequest request)
    {
        Deposit.Result result = _accountService.Deposit(sessionKey, new Deposit.Request(request.Amount));

        return result switch
        {
            Deposit.Result.Success success => Ok(new { success.NewBalance }),
            Deposit.Result.InvalidAmount => BadRequest(new { Message = "Invalid amount" }),
            Deposit.Result.AccountNotFound => NotFound(new { Message = "Account not found" }),
            Deposit.Result.Unauthorized => Unauthorized(new { Message = "Unauthorized" }),
            _ => BadRequest(),
        };
    }

    [HttpGet("operations")]
    public IActionResult Operations([FromHeader(Name = "X-Session-Key")] System.Guid sessionKey)
    {
        System.Collections.Generic.IReadOnlyCollection<Domain.Operations.Operation> operations =
            _accountService.GetOperationHistory(sessionKey);

        var resultList = new System.Collections.Generic.List<object>();
        foreach (Domain.Operations.Operation op in operations)
        {
            resultList.Add(new
            {
                Id = op.Id.Value,
                op.OperationType,
                Amount = op.Amount.Value,
                op.CreatedAt,
            });
        }

        return Ok(resultList);
    }
}