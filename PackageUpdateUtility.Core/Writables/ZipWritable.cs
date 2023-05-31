using System.IO.Compression;

namespace PackageUpdateUtility.Core.Writables;

public class ZipWritable : IWritable
{

    private string _path;
    private string _entryPath;

    private FileStream _zipFile;
    private ZipArchive _archiveStream;
    private Stream _stream;
    
    public ZipWritable(string path, string entryPath)
    {
        _path = path;
        _entryPath = entryPath;
    }
    
    public Stream OpenWrite()
    {
        _zipFile = new FileStream(_path, FileMode.Open);
        _archiveStream = new ZipArchive(_zipFile, ZipArchiveMode.Update);
        _stream = _archiveStream.GetEntry(_entryPath).Open();
        _stream.SetLength(0);
        return _stream;
    }

    public void Close()
    {
        _stream.Close();
        _archiveStream.Dispose();
        _zipFile.Close();
    }
}