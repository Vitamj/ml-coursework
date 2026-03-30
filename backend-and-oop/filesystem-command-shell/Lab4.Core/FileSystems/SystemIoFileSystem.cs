using System;
using System.IO;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;

public sealed class SystemIoFileSystem : IFileSystem
{
    public IEnumerable<string> List(IPath directory)
    {
        if (directory is null) throw new ArgumentNullException(nameof(directory));

        string dir = ToOsPath(directory);

        if (!Directory.Exists(dir))
            return Array.Empty<string>();


        IEnumerable<string> dirs = Directory.EnumerateDirectories(dir).Select(Path.GetFileName);
        IEnumerable<string> files = Directory.EnumerateFiles(dir).Select(Path.GetFileName);

        return dirs.Concat(files).Where(x => !string.IsNullOrWhiteSpace(x))!;
    }

    public bool DirectoryExists(IPath path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));
        return Directory.Exists(ToOsPath(path));
    }

    public bool FileExists(IPath path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));
        return File.Exists(ToOsPath(path));
    }

    public CommandResult ReadFile(IPath path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        string filePath = ToOsPath(path);

        if (!File.Exists(filePath))
            return new CommandResult.FileNotFound(filePath);

        try
        {
            string content = File.ReadAllText(filePath);
            return new CommandResult.FileContent(content);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return new CommandResult.Failure(ex.Message);
        }
    }

    public CommandResult CopyFile(IPath source, IPath destinationDirectory)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (destinationDirectory is null) throw new ArgumentNullException(nameof(destinationDirectory));

        string src = ToOsPath(source);
        if (!File.Exists(src))
            return new CommandResult.FileNotFound(src);

        string dstDir = ToOsPath(destinationDirectory);
        if (!Directory.Exists(dstDir))
            return new CommandResult.DirectoryNotFound(dstDir);

        string fileName = Path.GetFileName(src);
        string dstFile = Path.Combine(dstDir, fileName);

        if (File.Exists(dstFile))
            return new CommandResult.NameCollision(dstFile);

        try
        {
            File.Copy(src, dstFile);
            return new CommandResult.Success();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return new CommandResult.Failure(ex.Message);
        }
    }

    public CommandResult MoveFile(IPath source, IPath destinationDirectory)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (destinationDirectory is null) throw new ArgumentNullException(nameof(destinationDirectory));

        string src = ToOsPath(source);
        if (!File.Exists(src))
            return new CommandResult.FileNotFound(src);

        string dstDir = ToOsPath(destinationDirectory);
        if (!Directory.Exists(dstDir))
            return new CommandResult.DirectoryNotFound(dstDir);

        string fileName = Path.GetFileName(src);
        string dstFile = Path.Combine(dstDir, fileName);

        if (File.Exists(dstFile))
            return new CommandResult.NameCollision(dstFile);

        try
        {
            File.Move(src, dstFile);
            return new CommandResult.Success();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return new CommandResult.Failure(ex.Message);
        }
    }

    public CommandResult DeleteFile(IPath path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        string filePath = ToOsPath(path);

        if (!File.Exists(filePath))
            return new CommandResult.FileNotFound(filePath);

        try
        {
            File.Delete(filePath);
            return new CommandResult.Success();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return new CommandResult.Failure(ex.Message);
        }
    }

    public CommandResult RenameFile(IPath path, string newName)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));
        if (newName is null) throw new ArgumentNullException(nameof(newName));

        if (string.IsNullOrWhiteSpace(newName))
            return new CommandResult.Failure("New name cannot be empty.");


        if (newName.Contains('/') || newName.Contains('\\'))
            return new CommandResult.Failure("New name must be a file name, not a path.");

        string src = ToOsPath(path);
        if (!File.Exists(src))
            return new CommandResult.FileNotFound(src);

        string? dir = Path.GetDirectoryName(src);
        string dst = Path.Combine(dir ?? string.Empty, newName);

        if (File.Exists(dst))
            return new CommandResult.NameCollision(dst);

        try
        {
            File.Move(src, dst);
            return new CommandResult.Success();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return new CommandResult.Failure(ex.Message);
        }
    }

    private static string ToOsPath(IPath path)
        => path.ToString() ?? string.Empty;
}
