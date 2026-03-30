using System;
using System.IO;

namespace Lab2.Formatters;

public class FileFormatter : IMessageFormatter
{
    private readonly string _filePath;

    public FileFormatter(string filePath)
    {
        _filePath = filePath;
    }

    public void FormatHeader(string header)
    {
        File.AppendAllText(_filePath, header + Environment.NewLine);
    }

    public void FormatBody(string body)
    {
        File.AppendAllText(_filePath, body + Environment.NewLine);
    }
}