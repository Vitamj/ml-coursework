using Lab2.Formatters;
using Lab2.Results;
using Lab2.ValueObjects;
using System;

namespace Lab2.Archivers;

public class FormattingArchiver : IArchiver
{
    private readonly IMessageFormatter _formatter;

    public FormattingArchiver(IMessageFormatter formatter)
    {
        _formatter = formatter;
    }

    public void Archive(string title, string body, MessagePriorityValue priority)
    {
        _formatter.FormatHeader(title);
        _formatter.FormatBody(body);
    }
}