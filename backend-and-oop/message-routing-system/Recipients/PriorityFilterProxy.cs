using Lab2.Recipients;
using Lab2.Results;
using Lab2.ValueObjects;

namespace Lab2.Recipients;

public class PriorityFilterProxy : IRecipient
{
    private readonly IRecipient _recipient;
    private readonly MessagePriorityValue _minPriority;

    public PriorityFilterProxy(IRecipient recipient, MessagePriorityValue minPriority)
    {
        _recipient = recipient;
        _minPriority = minPriority;
    }

    public SendMessageResult Receive(string title, string body, MessagePriorityValue priority)
    {
        if (priority >= _minPriority)
        {
            return _recipient.Receive(title, body, priority);
        }

        return new SendMessageResult.Filtered();
    }
}