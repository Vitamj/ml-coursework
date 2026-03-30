namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed record Speed
{
    public double MetersPerSecond { get; }

    public Speed(double metersPerSecond)
    {
        MetersPerSecond = metersPerSecond;
    }

    public static Speed FromMetersPerSecond(double metersPerSecond) => new(metersPerSecond);

    public static bool operator >(Speed left, Speed right) => left.MetersPerSecond > right.MetersPerSecond;

    public static bool operator <(Speed left, Speed right) => left.MetersPerSecond < right.MetersPerSecond;

    public static bool operator >=(Speed left, Speed right) => left.MetersPerSecond >= right.MetersPerSecond;

    public static bool operator <=(Speed left, Speed right) => left.MetersPerSecond <= right.MetersPerSecond;

    public static bool operator <(Speed left, double right) => left.MetersPerSecond < right;

    public static Speed operator +(Speed left, Speed right) => new(left.MetersPerSecond + right.MetersPerSecond);
    public static Distance operator *(Speed left, Time right) => Distance.FromMeters(left.MetersPerSecond * right.Seconds);
}