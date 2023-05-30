using PackageUpdateUtility.Core.Writables;

namespace PackageUpdateUtility.Core.FileLoaders;

public class BasicFileLoader : FileLoader
{
    
    public override FileEnvironment Load(FileEnvironment fileEnvironment)
    {
        fileEnvironment.Data = File.ReadAllText(fileEnvironment.Path);
        fileEnvironment.FileWritable = new BasicFileWritable(fileEnvironment.Path);

        return fileEnvironment;
    }
    
}