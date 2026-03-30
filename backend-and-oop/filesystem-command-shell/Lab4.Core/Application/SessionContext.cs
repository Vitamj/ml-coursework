using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Output;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Application;

public sealed class SessionContext
{
    private readonly IPathParser _pathParser;

    public SessionContext(IOutput output, IPathParser pathParser)
    {
        Output = output ?? throw new ArgumentNullException(nameof(output));
        _pathParser = pathParser ?? throw new ArgumentNullException(nameof(pathParser));

        RootPath = _pathParser.Parse("/").Normalize();
        LocalPath = RootPath;
    }

    public IOutput Output { get; }

    public IFileSystem? FileSystem { get; private set; }

    public IPath RootPath { get; private set; }

    public IPath LocalPath { get; private set; }

    public bool IsConnected => FileSystem is not null;

    public CommandResult Connect(IFileSystem fileSystem, IPath root)
    {
        if (fileSystem is null) return new CommandResult.Failure("File system is null.");
        if (root is null) return new CommandResult.Failure("Root path is null.");

        FileSystem = fileSystem;
        RootPath = root.Normalize();
        LocalPath = RootPath;

        return new CommandResult.Success();
    }

    public CommandResult Disconnect()
    {
        if (!IsConnected) return new CommandResult.NotConnected();

        FileSystem = null;
        RootPath = _pathParser.Parse("/").Normalize();
        LocalPath = RootPath;

        return new CommandResult.Success();
    }

    public CommandResult ChangeDirectory(IPath path)
    {
        if (!IsConnected || FileSystem is null) return new CommandResult.NotConnected();
        if (path is null) return new CommandResult.Failure("Path is null.");

        ResolvePathResult resolved = ResolveVirtualPath(path, mustBeDirectory: true);
        if (resolved is ResolvePathResult.Failure fail) return fail.Error;

        var ok = (ResolvePathResult.Success)resolved;

        if (!FileSystem.DirectoryExists(ok.Path))
            return new CommandResult.DirectoryNotFound(ok.Path.ToString() ?? string.Empty);

        LocalPath = ok.Path;
        return new CommandResult.Success();
    }


    public CommandResult TryResolveFilePath(IPath virtualPath, out IPath? absolutePath)
    {
        absolutePath = null;

        if (!IsConnected || FileSystem is null)
            return new CommandResult.NotConnected();

        if (virtualPath is null)
            return new CommandResult.Failure("Path is null.");

        ResolvePathResult resolved = ResolveVirtualPath(virtualPath, mustBeDirectory: false);
        if (resolved is ResolvePathResult.Failure fail)
            return fail.Error;

        var ok = (ResolvePathResult.Success)resolved;

        if (!FileSystem.FileExists(ok.Path))
            return new CommandResult.FileNotFound(ok.Path.ToString() ?? string.Empty);

        absolutePath = ok.Path;
        return new CommandResult.Success();
    }


    public ResolvePathResult ResolveVirtualPath(IPath virtualPath, bool mustBeDirectory)
    {
        if (!IsConnected || FileSystem is null)
            return new ResolvePathResult.Failure(new CommandResult.NotConnected());

        if (virtualPath is null)
            return new ResolvePathResult.Failure(new CommandResult.Failure("Path is null."));


        IPath basePath = virtualPath.IsAbsolute ? RootPath : LocalPath;

        IPath relativePart = virtualPath;

        if (virtualPath.IsAbsolute)
        {
            string text = virtualPath.ToString() ?? string.Empty;
            string trimmed = text.TrimStart('/');

            relativePart = _pathParser.Parse(string.IsNullOrWhiteSpace(trimmed) ? "." : trimmed);
        }

        IPath combined;
        try
        {
            combined = basePath.CombineRelative(relativePart.Normalize()).Normalize();
        }
        catch (InvalidOperationException)
        {
            return new ResolvePathResult.Failure(
                new CommandResult.InvalidPath(virtualPath.ToString() ?? string.Empty));
        }

        if (!IsUnderRoot(RootPath, combined))
            return new ResolvePathResult.Failure(
                new CommandResult.InvalidPath(combined.ToString() ?? string.Empty));

        if (mustBeDirectory && !FileSystem.DirectoryExists(combined))
            return new ResolvePathResult.Failure(
                new CommandResult.DirectoryNotFound(combined.ToString() ?? string.Empty));

        return new ResolvePathResult.Success(combined);
    }

    private static bool IsUnderRoot(IPath root, IPath candidate)
    {
        string r = root.ToString() ?? string.Empty;
        string c = candidate.ToString() ?? string.Empty;

        if (r == "/") return c.StartsWith("/", StringComparison.Ordinal);
        if (c.Equals(r, StringComparison.Ordinal)) return true;
        return c.StartsWith(r.TrimEnd('/') + "/", StringComparison.Ordinal);
    }
}

public abstract record ResolvePathResult
{
    private ResolvePathResult() { }

    public sealed record Success(IPath Path) : ResolvePathResult;

    public sealed record Failure(CommandResult Error) : ResolvePathResult;
}
