namespace Lab2.Results;

public abstract record MarkAsReadResult
{
    private MarkAsReadResult() { }

    public sealed record Success : MarkAsReadResult;
    public sealed record AlreadyRead : MarkAsReadResult;
}