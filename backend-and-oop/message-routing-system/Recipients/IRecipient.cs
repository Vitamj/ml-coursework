using Lab2.Results;
using Lab2.ValueObjects;

namespace Lab2.Recipients;

public interface IRecipient
{
    SendMessageResult Receive(string title, string body, MessagePriorityValue priority);
}