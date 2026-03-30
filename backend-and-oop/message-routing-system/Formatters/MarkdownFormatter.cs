using System;

namespace Lab2.Formatters;

public class MarkdownFormatter : IMessageFormatter
{
    private readonly IMessageFormatter _decoratee;

    public MarkdownFormatter(IMessageFormatter decoratee)
    {
        _decoratee = decoratee;
    }

    public void FormatHeader(string header)
    {
        _decoratee.FormatHeader($"# {header}");
        _decoratee.FormatHeader("");
    }

    public void FormatBody(string body)
    {
        _decoratee.FormatBody(body);
        _decoratee.FormatBody("");
    }
}