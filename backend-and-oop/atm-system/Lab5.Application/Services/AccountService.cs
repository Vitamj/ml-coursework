using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Queries;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts.Operations;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Operations;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Services;

public class AccountService : IAccountService
{
    private readonly IPersistenceContext _context;
    private readonly ISessionService _sessionService;
    private long _nextAccountId = 1;
    private long _nextOperationId = 1;

    public AccountService(IPersistenceContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public CreateAccount.Result CreateAccount(System.Guid sessionKey, CreateAccount.Request request)
    {
        if (!_sessionService.IsValidAdminSession(sessionKey))
            return new CreateAccount.Result.Unauthorized();

        Account? existing = _context.Accounts.Query(new AccountQuery(AccountNumber: request.AccountNumber));
        if (existing is not null)
            return new CreateAccount.Result.AccountAlreadyExists();

        var accountId = new AccountId(_nextAccountId++);
        var account = new Account(accountId, request.AccountNumber, request.PinCode, Money.Zero);
        _context.Accounts.Add(account);

        return new CreateAccount.Result.Success(accountId.Value);
    }

    public GetBalance.Result GetBalance(System.Guid sessionKey)
    {
        if (!_sessionService.IsValidUserSession(sessionKey))
            return new GetBalance.Result.Unauthorized();

        long? accountIdValue = _sessionService.GetAccountIdFromSession(sessionKey);
        if (accountIdValue is null)
            return new GetBalance.Result.AccountNotFound();

        var accountId = new AccountId(accountIdValue.Value);
        Account? account = _context.Accounts.Query(new AccountQuery(Id: accountId));
        if (account is null)
            return new GetBalance.Result.AccountNotFound();

        return new GetBalance.Result.Success(account.Balance.Value);
    }

    public Withdraw.Result Withdraw(System.Guid sessionKey, Withdraw.Request request)
    {
        if (!_sessionService.IsValidUserSession(sessionKey))
            return new Withdraw.Result.Unauthorized();

        if (request.Amount <= 0)
            return new Withdraw.Result.InvalidAmount();

        long? accountIdValue = _sessionService.GetAccountIdFromSession(sessionKey);
        if (accountIdValue is null)
            return new Withdraw.Result.AccountNotFound();

        var accountId = new AccountId(accountIdValue.Value);
        Account? account = _context.Accounts.Query(new AccountQuery(Id: accountId));
        if (account is null)
            return new Withdraw.Result.AccountNotFound();

        var amount = new Money(request.Amount);
        if (!account.Withdraw(amount))
            return new Withdraw.Result.InsufficientFunds();

        _context.Accounts.Update(account);

        var operationId = new OperationId(_nextOperationId++);
        var operation = new Operation(operationId, accountId, OperationType.Withdrawal, amount);
        _context.Operations.Add(operation);

        return new Withdraw.Result.Success(account.Balance.Value);
    }

    public Deposit.Result Deposit(System.Guid sessionKey, Deposit.Request request)
    {
        if (!_sessionService.IsValidUserSession(sessionKey))
            return new Deposit.Result.Unauthorized();

        if (request.Amount <= 0)
            return new Deposit.Result.InvalidAmount();

        long? accountIdValue = _sessionService.GetAccountIdFromSession(sessionKey);
        if (accountIdValue is null)
            return new Deposit.Result.AccountNotFound();

        var accountId = new AccountId(accountIdValue.Value);
        Account? account = _context.Accounts.Query(new AccountQuery(Id: accountId));
        if (account is null)
            return new Deposit.Result.AccountNotFound();

        var amount = new Money(request.Amount);
        account.Deposit(amount);
        _context.Accounts.Update(account);

        var operationId = new OperationId(_nextOperationId++);
        var operation = new Operation(operationId, accountId, OperationType.Deposit, amount);
        _context.Operations.Add(operation);

        return new Deposit.Result.Success(account.Balance.Value);
    }

    public System.Collections.Generic.IReadOnlyCollection<Operation> GetOperationHistory(System.Guid sessionKey)
    {
        if (!_sessionService.IsValidUserSession(sessionKey))
            return System.Array.Empty<Operation>();

        long? accountIdValue = _sessionService.GetAccountIdFromSession(sessionKey);
        if (accountIdValue is null)
            return System.Array.Empty<Operation>();

        var accountId = new AccountId(accountIdValue.Value);
        return _context.Operations.Query(new OperationQuery(AccountId: accountId));
    }
}
