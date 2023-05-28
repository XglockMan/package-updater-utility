namespace PackageUpdateUtility.Core;

public class FileEnvironment
{

    private Application _application;

    public FileEnvironment(Application application, string path, Modifier modifier, FileLoader loader)
    {
        _application = application;
        Modifier = modifier;
        FileLoader = loader;
        Path = path;
    }
    
    public string Path { get; private set; }
    
    public FileLoader FileLoader { get; private set; }
    
    public Stream WriteStream { get; protected internal set; }
    
    public string Data { get; protected internal set; }
    
    public Modifier Modifier { get; protected set; }


}