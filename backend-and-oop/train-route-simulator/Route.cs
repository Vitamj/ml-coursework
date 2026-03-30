using Itmo.ObjectOrientedProgramming.Lab1.Results;
using Itmo.ObjectOrientedProgramming.Lab1.Segments;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1;

public class Route
{
    private readonly IReadOnlyCollection<IRouteSegment> _segments;
    private readonly Speed _maxFinalSpeed;

    public Route(Speed maxFinalSpeed, IReadOnlyCollection<IRouteSegment> segments)
    {
        _maxFinalSpeed = maxFinalSpeed;
        _segments = segments;
    }

    public SimulateRouteResult Simulate(Train train)
    {
        Time totalTime = Time.FromSeconds(0);

        foreach (IRouteSegment segment in _segments)
        {
            PassSegmentResult result = segment.Pass(train);

            switch (result)
            {
                case PassSegmentResult.Success success:
                    totalTime = totalTime + success.Duration;
                    break;
                case PassSegmentResult.ForceLimitExceeded:
                    return new SimulateRouteResult.Failure("Applied force exceeds maximum allowed");
                case PassSegmentResult.SpeedLimitExceeded:
                    return new SimulateRouteResult.Failure("Speed exceeds segment limit");
                case PassSegmentResult.TrainNotMoving:
                    return new SimulateRouteResult.Failure("Train cannot move in current conditions");
                default:
                    return new SimulateRouteResult.Failure("Unknown error during segment passage");
            }
        }

        if (train.CurrentSpeed > _maxFinalSpeed)
            return new SimulateRouteResult.Failure("Final speed exceeds route limit");

        return new SimulateRouteResult.Success(totalTime);
    }
}