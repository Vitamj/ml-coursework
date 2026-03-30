using Lab2.Results;
using Lab2.ValueObjects;

namespace Lab2.Archivers;

public interface IArchiver
{
    ArchiveResult Archive(string title, string body, MessagePriorityValue priority);
}