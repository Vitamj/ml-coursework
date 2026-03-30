namespace Lab2.Messages;

public sealed record MessageBody
{
    public string Value { get; }

    public MessageBody(string value)
    {
        Value = value ?? string.Empty;
    }

    public static MessageBody FromString(string value) => new(value);
}