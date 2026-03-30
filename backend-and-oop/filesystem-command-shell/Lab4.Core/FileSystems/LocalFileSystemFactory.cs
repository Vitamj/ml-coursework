namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;

public sealed class LocalFileSystemFactory : IFileSystemFactory
{
    public IFileSystem Create(string mode)
    {
        if (!string.Equals(mode, "local", StringComparison.Ordinal))
            return new SystemIoFileSystem(); 
        return new SystemIoFileSystem();
    }
}
