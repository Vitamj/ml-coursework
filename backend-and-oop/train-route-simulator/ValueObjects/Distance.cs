namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed record Distance
{
    public double Meters { get; }

    public Distance(double meters)
    {
        if (meters < 0)
            throw new ArgumentException("Distance cannot be negative", nameof(meters));
        Meters = meters;
    }

    public static Distance FromMeters(double meters) => new(meters);

        public static bool operator >(Distance left, double right) => left.Meters > right;
    public static bool operator <(Distance left, double right) => left.Meters < right;
    public static bool operator <=(Distance left, double right) => left.Meters <= right;
    public static bool operator >=(Distance left, double right) => left.Meters >= right;

    public static Distance operator -(Distance left, Distance right) => new(left.Meters - right.Meters);
    public static Time operator /(Distance left, Speed right) => Time.FromSeconds(left.Meters / right.MetersPerSecond);
}