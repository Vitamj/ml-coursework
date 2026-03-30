using Lab2.ValueObjects;

namespace Lab2.Messages;

public class Message
{
    public Message(MessageHeader header, MessageBody body, MessagePriorityValue priority)
    {
        Header = header;
        Body = body;
        Priority = priority;
    }

    public MessageHeader Header { get; }
    public MessageBody Body { get; }
    public MessagePriorityValue Priority { get; }
}