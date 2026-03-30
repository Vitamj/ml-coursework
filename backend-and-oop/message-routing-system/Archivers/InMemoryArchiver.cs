using Lab2.Core;
using Lab2.Results;
using Lab2.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Archivers;

public class InMemoryArchiver : IArchiver
{
    private readonly List<Message> _archivedMessages;

    public InMemoryArchiver()
    {
        _archivedMessages = new List<Message>();
    }

    public IReadOnlyCollection<Message> ArchivedMessages => _archivedMessages.AsReadOnly();

    public ArchiveResult Archive(string title, string body, MessagePriorityValue priority)
    {
        var header = new MessageHeader(title);
        var messageBody = new MessageBody(body);
        var message = new Message(header, messageBody, priority);

        _archivedMessages.Add(message);
        return new ArchiveResult.Success();
    }
}