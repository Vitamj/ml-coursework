using Itmo.ObjectOrientedProgramming.Lab5.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Operations.Results;

public abstract record GetBalanceResult
{
    private GetBalanceResult() { }

    public sealed record Success(Money Balance) : GetBalanceResult;

    public sealed record AccountNotFound : GetBalanceResult;

    public sealed record Unauthorized : GetBalanceResult;
}