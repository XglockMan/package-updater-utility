namespace PackageUpdateUtility.Core;

public interface ILoaderWriter
{
    void Load(FileEnvironment fileEnvironment, Action<Stream> action);
    void Write(FileEnvironment fileEnvironment, Action<Stream> action);

}