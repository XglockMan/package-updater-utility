using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Tests;

public class TestFileLoader : FileLoader
{
    public IWritable TestWritable = new TestWritable();
        
    public override FileEnvironment Load(FileEnvironment fileEnvironment)
    {
        fileEnvironment.Data = fileEnvironment.Path;

        fileEnvironment.FileWritable = TestWritable;
            
        return fileEnvironment;
    }
}