using Lab2.Core;
using Lab2.Results;
using Lab2.ValueObjects;

namespace Lab2.Recipients;

public class UserRecipient : IRecipient
{
    private readonly User _user;

    public UserRecipient(User user)
    {
        _user = user;
    }

    public SendMessageResult Receive(string title, string body, MessagePriorityValue priority)
    {
        var header = new MessageHeader(title);
        var messageBody = new MessageBody(body);
        var message = new Message(header, messageBody, priority);

        _user.Receive(message);
        return new SendMessageResult.Success();
    }
}