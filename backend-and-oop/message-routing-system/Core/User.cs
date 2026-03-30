using Lab2.Results;
using Lab2.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Core;

public class User
{
    private readonly List<MessageStatus> _messages;

    public User(string name)
    {
        Name = name;
        _messages = new List<MessageStatus>();
    }

    public string Name { get; }
    public IReadOnlyCollection<MessageStatus> ReceivedMessages => _messages.AsReadOnly();

    public void Receive(Message message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));

        _messages.Add(new MessageStatus(message.Header, message.Body, message.Priority));
    }

    public MarkAsReadResult MarkAsRead(Message message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));

        MessageStatus? messageStatus = _messages
            .FirstOrDefault(msg =>
                msg.Header.Value == message.Header.Value &&
                msg.Body.Value == message.Body.Value);

        return messageStatus?.MarkAsRead() ?? new MarkAsReadResult.AlreadyRead();
    }
}