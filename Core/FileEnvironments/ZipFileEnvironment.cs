using System.IO.Compression;
using System.Text;

namespace Core.FileEnvironments;

public class ZipFileEnvironment : IFileEnvironment
{

    private string _path;
    private Application _application;
    private Type _modifier;
    
    protected ZipFileEnvironment(Application application, string path, Type modifier)
    {
        _path = path;
        _application = application;
        _modifier = modifier;
    }

    protected void Load()
    {
        int zipPathIndex = _path.LastIndexOf(".zip", StringComparison.Ordinal);

        string filePath = _path.Substring(0, zipPathIndex);
        string zipPath = _path.Substring(zipPathIndex);

        using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                WriteStream = archive.CreateEntry(zipPath).Open();
            }
        }
    }
    
    public Stream WriteStream { get; set; }
    public string Data { get; set; }
    public Type Modificator { get; set; }
}