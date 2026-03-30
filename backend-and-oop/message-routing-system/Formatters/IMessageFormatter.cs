using System;

namespace Lab2.Formatters;

namespace Lab2.Formatters;

public interface IMessageFormatter
{
    void FormatHeader(string header);
    void FormatBody(string body);
}