namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

public sealed class UnixPathParser : IPathParser
{
    private static readonly char[] Separator = ['/'];

    public IPath Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Path cannot be empty.", nameof(text));

        text = text.Trim();

        bool isAbsolute = text.StartsWith("/", StringComparison.Ordinal);
        string trimmed = text.Trim('/');

        string[] segments = trimmed.Length == 0
            ? Array.Empty<string>()
            : trimmed.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

        return new UnixPath(isAbsolute, segments);
    }
}
