using System.IO.Compression;

namespace PackageUpdateUtility.Core.FileLoaders;

public class ZipFileLoader : FileLoader
{
    
    public override FileEnvironment Load(FileEnvironment fileEnvironment)
    {
        
        int zipIndex = fileEnvironment.Path.LastIndexOf(".zip", StringComparison.Ordinal) + 4;

        string filePath = fileEnvironment.Path.Substring(0, zipIndex);
        string zipPath = fileEnvironment.Path.Substring(zipIndex + 1);
        
        using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
            {
                StreamReader streamReader = new StreamReader(archive.GetEntry(zipPath).Open());

                fileEnvironment.Data = streamReader.ReadToEnd();

            }
            
            zipToOpen.Close();
        }
        
        using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                fileEnvironment.WriteStream = archive.GetEntry(zipPath).Open();
            }
        }

        return fileEnvironment;
        
    }
}