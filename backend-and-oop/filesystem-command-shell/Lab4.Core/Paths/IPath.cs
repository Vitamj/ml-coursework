namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

public interface IPath
{
    bool IsAbsolute { get; }
    IReadOnlyList<string> Segments { get; }

    IPath Normalize();
    IPath CombineRelative(IPath relative);
}
