namespace PackageUpdateUtility.Core.FileLoaders;

public class BasicFileLoader : FileLoader
{
    
    public override FileEnvironment Load(FileEnvironment fileEnvironment)
    {
        fileEnvironment.Data = File.ReadAllText(fileEnvironment.Path);
        fileEnvironment.WriteStream = File.OpenWrite(fileEnvironment.Path);

        return fileEnvironment;
    }
    
}