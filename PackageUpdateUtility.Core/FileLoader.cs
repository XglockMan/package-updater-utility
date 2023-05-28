namespace PackageUpdateUtility.Core;

public abstract class FileLoader
{

    public FileLoader()
    {
    }
    
    public abstract FileEnvironment Load(FileEnvironment fileEnvironment);

}