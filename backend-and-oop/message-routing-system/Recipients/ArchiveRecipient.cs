using Lab2.Archivers;
using Lab2.Results;
using Lab2.ValueObjects;

namespace Lab2.Recipients;

public class ArchiveRecipient : IRecipient
{
    private readonly IArchiver _archiver;

    public ArchiveRecipient(IArchiver archiver)
    {
        _archiver = archiver;
    }

    public SendMessageResult Receive(string title, string body, MessagePriorityValue priority)
    {
        ArchiveResult result = _archiver.Archive(title, body, priority);
        return result switch
        {
            ArchiveResult.Success => new SendMessageResult.Success(),
            ArchiveResult.Error error => new SendMessageResult.Error(error.Message),
            _ => new SendMessageResult.Error("Unknown archiving error"),
        };
    }
}