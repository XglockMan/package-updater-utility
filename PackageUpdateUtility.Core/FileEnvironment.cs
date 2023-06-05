namespace PackageUpdateUtility.Core;

public class FileEnvironment
{

    private Application _application;

    public FileEnvironment(Application application, string path, Modifier modifier, ILoaderWriter loaderWriter)
    {
        _application = application;
        Modifier = modifier;
        LoaderWriter = loaderWriter;
        Path = path;
    }
    
    public string Path { get; private set; }
    
    public ILoaderWriter LoaderWriter { get; private set; }
    
    public object ParsedData { get; set; }
    
    public Modifier Modifier { get; protected set; }


}