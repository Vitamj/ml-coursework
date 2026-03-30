using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Results;

public abstract record SimulateRouteResult
{
    private SimulateRouteResult() { }

    public sealed record Success(Time TotalTime) : SimulateRouteResult;

    public sealed record Failure(string Error) : SimulateRouteResult;
}