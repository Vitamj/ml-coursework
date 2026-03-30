using System;

namespace Lab2.Formatters;

public class ConsoleFormatter : IMessageFormatter
{
    public void FormatHeader(string header)
    {
        Console.WriteLine(header);
    }

    public void FormatBody(string body)
    {
        Console.WriteLine(body);
    }
}