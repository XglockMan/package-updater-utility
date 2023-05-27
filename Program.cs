// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using PackageUpdateUtility;
using PackageUpdateUtility.Modifications;

const string newLoggingUrl = "https://seq-log.eposid.eu/";

// init const delegates
IModification.OpenFile openFileDelegate = (path) => { return File.Open(path, FileMode.Open); };
IModification.OpenFile zipOpenDelegate = (path) =>
{

    string[] pathParts = path.Split("$");
    
    if (pathParts.Length != 2)
    {
        throw new Exception($"Invalid zip path {path}");
    }
    
    string zipPath = pathParts[0];
    string zipEntryPath = pathParts[1];
    
    using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
    {
        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
        {
            ZipArchiveEntry readmeEntry = archive.CreateEntry(zipEntryPath);

            return readmeEntry.Open();

        }
    }
};

// init modificator
Modificator modificator = new Modificator();

// add modifications
modificator.Modifications.Add(new UpdateConfigModification(newLoggingUrl, "HaSaM/Epos.Server/Services/Windows/Epos/LocalConfiguration/AppSettings.config", openFileDelegate));
modificator.Modifications.Add(new LocalConfigurationModification("[old]-A", "HaSaM/Epos.Server/Services/IIS/UpdateWeb/Packages/Epos/package.xml", openFileDelegate));
modificator.Modifications.Add(new LocalConfigurationModification(newLoggingUrl, "HaSaM/Epos.Server/Services/IIS/UpdateWeb/Packages/Epos/Epos.zip$LocalConfiguration/AppSettings.config", zipOpenDelegate));

// run modifications
modificator.RunAllModifications();