using Itmo.ObjectOrientedProgramming.Lab5.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Operations.Results;

public abstract record WithdrawResult
{
    private WithdrawResult() { }

    public sealed record Success(Money NewBalance) : WithdrawResult;

    public sealed record InsufficientFunds : WithdrawResult;

    public sealed record InvalidAmount : WithdrawResult;

    public sealed record AccountNotFound : WithdrawResult;

    public sealed record Unauthorized : WithdrawResult;
}