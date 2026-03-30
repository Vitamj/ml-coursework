using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;

public interface IFileSystem
{
    IEnumerable<string> List(IPath directory);

    bool DirectoryExists(IPath path);
    bool FileExists(IPath path);

    CommandResult ReadFile(IPath path);

    CommandResult CopyFile(IPath source, IPath destinationDirectory);
    CommandResult MoveFile(IPath source, IPath destinationDirectory);
    CommandResult DeleteFile(IPath path);
    CommandResult RenameFile(IPath path, string newName);
}
