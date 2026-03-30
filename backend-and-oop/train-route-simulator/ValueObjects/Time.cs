namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed record Time
{
    public double Seconds { get; }

    public Time(double seconds)
    {
        if (seconds < 0)
            throw new ArgumentException("Time cannot be negative", nameof(seconds));
        Seconds = seconds;
    }

    public static Time FromSeconds(double seconds) => new(seconds);

    public static Time operator +(Time left, Time right) => new(left.Seconds + right.Seconds);
}