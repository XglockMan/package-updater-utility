using System.IO.Compression;
using PackageUpdateUtility.Core.Writables;

namespace PackageUpdateUtility.Core.FileLoaders;

public class ZipFileLoader : FileLoader
{

    public static void ParseZipPath(string path,  out string zipPath, out string zipEntryPath)
    {
        
        int zipIndex = path.LastIndexOf(".zip", StringComparison.Ordinal) + 4;

        zipPath = path.Substring(0, zipIndex);
        zipEntryPath = path.Substring(zipIndex + 1);
    }
    
    public override FileEnvironment Load(FileEnvironment fileEnvironment)
    {
        
        ParseZipPath(fileEnvironment.Path, out string zipPath, out string zipEntryPath);
        
        using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
            {
                StreamReader streamReader = new StreamReader(archive.GetEntry(zipEntryPath).Open());

                fileEnvironment.Data = streamReader.ReadToEnd();

            }
        }
        
        fileEnvironment.FileWritable = new ZipWritable(zipPath, zipEntryPath);

        return fileEnvironment;
        
    }
}