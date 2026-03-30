using Lab2.NotificationSystems;
using Lab2.Results;
using Lab2.ValueObjects;
using System.Linq;

namespace Lab2.Recipients;

public class NotificationSystemRecipient : IRecipient
{
    private readonly INotificationSystem _notificationSystem;
    private readonly IReadOnlyCollection<string> _suspiciousWords;

    public NotificationSystemRecipient(
        INotificationSystem notificationSystem,
        IReadOnlyCollection<string> suspiciousWords)
    {
        _notificationSystem = notificationSystem;
        _suspiciousWords = suspiciousWords;
    }

    public SendMessageResult Receive(string title, string body, MessagePriorityValue priority)
    {
        string textToCheck = $"{title} {body}".ToLowerInvariant();

        if (_suspiciousWords.Any(word =>
            textToCheck.Contains(word.ToLowerInvariant())))
        {
            _notificationSystem.Notify();
        }

        return new SendMessageResult.Success();
    }
}