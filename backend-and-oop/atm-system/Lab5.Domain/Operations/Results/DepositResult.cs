using Itmo.ObjectOrientedProgramming.Lab5.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Operations.Results;

public abstract record DepositResult
{
    private DepositResult() { }

    public sealed record Success(Money NewBalance) : DepositResult;

    public sealed record InvalidAmount : DepositResult;

    public sealed record AccountNotFound : DepositResult;

    public sealed record Unauthorized : DepositResult;
}