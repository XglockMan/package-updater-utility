using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Tests;

public class TestWritable : IWritable
{
    public MemoryStream MemoryStream = new();
        
    public Stream OpenWrite()
    {
        return MemoryStream;
    }

    public void Close()
    {
        MemoryStream.Close();
    }
}