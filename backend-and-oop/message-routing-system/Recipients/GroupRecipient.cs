using Lab2.Results;
using Lab2.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Recipients;

public class GroupRecipient : IRecipient
{
    private readonly IReadOnlyCollection<IRecipient> _recipients;

    public GroupRecipient(string groupName, IReadOnlyCollection<IRecipient> recipients)
    {
        GroupName = groupName;
        _recipients = recipients;
    }

    public string GroupName { get; }

    public void Receive(string title, string body, MessagePriorityValue priority)
    {
        foreach (IRecipient recipient in _recipients)
        {
            recipient.Receive(title, body, priority);
        }
    }

    public IReadOnlyCollection<IRecipient> GetRecipients() => _recipients;
}