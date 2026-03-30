using Lab2.Core;

namespace Lab2.Messages;

public sealed record MessagePriorityValue
{
    public MessagePriority Value { get; }

    public MessagePriorityValue(MessagePriority value)
    {
        Value = value;
    }

    public static MessagePriorityValue FromPriority(MessagePriority priority) => new(priority);

    public static bool operator >=(MessagePriorityValue left, MessagePriorityValue right)
        => (int)left.Value >= (int)right.Value;

    public static bool operator <=(MessagePriorityValue left, MessagePriorityValue right)
        => (int)left.Value <= (int)right.Value;
}