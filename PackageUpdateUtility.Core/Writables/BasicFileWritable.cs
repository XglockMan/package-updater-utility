namespace PackageUpdateUtility.Core.Writables;

public class BasicFileWritable : IWritable
{

    private string _filePath;

    private Stream _stream;
    
    public BasicFileWritable(string path)
    {
        _filePath = path;
    }

    public Stream OpenWrite()
    {
        _stream = File.OpenWrite(_filePath);
        return _stream;
    }

    public void Close()
    {
        _stream.Close();
    }
}