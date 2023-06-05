namespace PackageUpdateUtility.Core.FileLoaders;

public class BasicFileLoaderWriter : ILoaderWriter
{
    public void Load(FileEnvironment fileEnvironment, Action<Stream> action)
    {
        using (var _stream = File.Open(fileEnvironment.Path, FileMode.Open))
        {
            action(_stream);
        }
    }

    public void Write(FileEnvironment fileEnvironment, Action<Stream> action)
    {
        using (var _stream = File.Open(fileEnvironment.Path, FileMode.Truncate))
        {
            action(_stream);
        }
    }

}