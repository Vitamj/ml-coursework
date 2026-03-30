namespace Lab2.Messages;

public sealed record MessageHeader
{
    public string Value { get; }

    public MessageHeader(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Message header cannot be empty", nameof(value));

        Value = value;
    }

    public static MessageHeader FromString(string value) => new(value);
}