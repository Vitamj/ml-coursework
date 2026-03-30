using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Results;

public abstract record PassSegmentResult
{
    private PassSegmentResult() { }

    public sealed record Success(Time Duration) : PassSegmentResult;

    public sealed record TrainNotMoving : PassSegmentResult;

    public sealed record ForceLimitExceeded : PassSegmentResult;

    public sealed record SpeedLimitExceeded : PassSegmentResult;
}