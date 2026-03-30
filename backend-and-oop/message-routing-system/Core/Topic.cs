using Lab2.Recipients;
using Lab2.Results;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Core;

public class Topic
{
    private readonly IReadOnlyCollection<IRecipient> _recipients;

    public Topic(string name, IReadOnlyCollection<IRecipient> recipients)
    {
        Name = name;
        _recipients = recipients;
    }

    public string Name { get; }

    public void Send(Message message)
    {
        foreach (IRecipient recipient in _recipients)
        {
            recipient.Receive(message.Header.Value, message.Body.Value, message.Priority);
        }  
    }

    public IReadOnlyCollection<IRecipient> GetRecipients() => _recipients;
}