namespace Lab2.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine("[LOG] " + message);
    }
}