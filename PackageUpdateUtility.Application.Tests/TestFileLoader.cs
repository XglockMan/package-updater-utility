using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Tests;

public class TestFileLoader : ILoaderWriter
{
    public MemoryStream MemoryStream = new();

    public void Load(FileEnvironment fileEnvironment, Action<Stream> action)
    {
        action(MemoryStream);
    }

    public void Write(FileEnvironment fileEnvironment, Action<Stream> action)
    {
        action(MemoryStream);
    }
}