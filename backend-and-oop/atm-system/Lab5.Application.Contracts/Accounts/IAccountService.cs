using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts.Operations;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;

public interface IAccountService
{
    CreateAccount.Result CreateAccount(System.Guid sessionKey, CreateAccount.Request request);

    GetBalance.Result GetBalance(System.Guid sessionKey);

    Withdraw.Result Withdraw(System.Guid sessionKey, Withdraw.Request request);

    Deposit.Result Deposit(System.Guid sessionKey, Deposit.Request request);

    System.Collections.Generic.IReadOnlyCollection<Domain.Operations.Operation> GetOperationHistory(System.Guid sessionKey);
}