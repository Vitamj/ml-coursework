using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Operations;

public class Operation
{
    public Operation(OperationId id, AccountId accountId, OperationType operationType, Money amount)
    {
        Id = id;
        AccountId = accountId;
        OperationType = operationType;
        Amount = amount;
        CreatedAt = System.DateTime.UtcNow;
    }

    public OperationId Id { get; }

    public AccountId AccountId { get; }

    public OperationType OperationType { get; }

    public Money Amount { get; }

    public System.DateTime CreatedAt { get; }
}