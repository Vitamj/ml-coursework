namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;

public interface IFileSystemFactory
{
    IFileSystem Create(string mode);
}
