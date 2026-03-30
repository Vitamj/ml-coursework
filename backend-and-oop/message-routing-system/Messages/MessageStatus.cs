using Lab2.Results;
using Lab2.ValueObjects;

namespace Lab2.Messages;

public class MessageStatus
{
    public MessageStatus(MessageHeader header, MessageBody body, MessagePriorityValue priority)
    {
        Header = header;
        Body = body;
        Priority = priority;
        IsRead = false;
    }

    public MessageHeader Header { get; }
    public MessageBody Body { get; }
    public MessagePriorityValue Priority { get; }
    public bool IsRead { get; private set; }

    public MarkAsReadResult MarkAsRead()
    {
        if (IsRead)
        {
            return new MarkAsReadResult.AlreadyRead();
        }

        IsRead = true;
        return new MarkAsReadResult.Success();
    }
}