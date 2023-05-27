namespace Core;

public abstract class FileLoader
{

    private string _filePath;

    public FileLoader(string filePath)
    {
        _filePath = filePath;
    }
    
    public abstract IFileEnvironment Load();

}