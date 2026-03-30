using Lab2.Logging;
using Lab2.Recipients;
using Lab2.Results;
using Lab2.ValueObjects;

namespace Lab2.Recipients;

public class LoggingDecorator : IRecipient
{
    private readonly IRecipient _recipient;
    private readonly ILogger _logger;

    public LoggingDecorator(IRecipient recipient, ILogger logger)
    {
        _recipient = recipient;
        _logger = logger;
    }

    public SendMessageResult Receive(string title, string body, MessagePriorityValue priority)
    {
        _logger.Log($"Received message: {title}");
        return _recipient.Receive(title, body, priority);
    }
}