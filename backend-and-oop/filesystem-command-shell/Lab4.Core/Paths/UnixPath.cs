namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

public sealed class UnixPath : IPath
{
    public UnixPath(bool isAbsolute, IReadOnlyList<string> segments)
    {
        IsAbsolute = isAbsolute;
        Segments = segments ?? throw new ArgumentNullException(nameof(segments));
    }

    public bool IsAbsolute { get; }

    public IReadOnlyList<string> Segments { get; }

    public IPath Normalize()
    {
        var stack = new List<string>();

        foreach (string seg in Segments)
        {
            if (seg == ".") continue;

            if (seg == "..")
            {
                if (stack.Count > 0) stack.RemoveAt(stack.Count - 1);
                continue;
            }

            stack.Add(seg);
        }

        return new UnixPath(IsAbsolute, stack);
    }

    public IPath CombineRelative(IPath relative)
    {
        if (relative is null) throw new ArgumentNullException(nameof(relative));
        if (relative.IsAbsolute) throw new InvalidOperationException("Expected relative path.");

        return new UnixPath(IsAbsolute, Segments.Concat(relative.Segments).ToArray());
    }

    public override string ToString()
    {
        if (Segments.Count == 0) return IsAbsolute ? "/" : string.Empty;
        string joined = string.Join("/", Segments);
        return IsAbsolute ? "/" + joined : joined;
    }
}
