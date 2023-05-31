// See https://aka.ms/new-console-template for more information

using PackageUpdateUtility.Core;
using PackageUpdateUtility.Core.FileLoaders;
using PackageUpdateUtility.Modifiers;

Application application = Application.InitWithBasics();

application.RegisterModifier<LoggingUrlModifier>("https://seq-log.eposid.eu/");
application.RegisterModifier<VersionNumberModifier>("-A");

application.RegisterFile<BasicFileLoader, LoggingUrlModifier>("Epos.Server/Services/Windows/Epos/LocalConfiguration/AppSettings.config");
application.RegisterFile<ZipFileLoader, LoggingUrlModifier>("Epos.Server/Services/IIS/UpdateWeb/Packages/Epos/Epos.zip/LocalConfiguration/AppSettings.config");
application.RegisterFile<BasicFileLoader, VersionNumberModifier>("Epos.Server/Services/IIS/UpdateWeb/Packages/Epos/package.xml");

application.LoadFiles();

application.VerifyFiles();

if (application.FilesToBeModified.Count > 0)
{
    Console.WriteLine("These files are goind to be modified:");
    Console.ForegroundColor = ConsoleColor.Green;

    foreach (FileEnvironment fileToByModified in application.FilesToBeModified)
    {
        Console.WriteLine($" + {fileToByModified.Path}");
    }
    
    Console.ForegroundColor = ConsoleColor.White;
}

if (application.FilesNotToBeModified.Count > 0)
{
    Console.WriteLine("These files are up-to-date and will be not modified:");
    Console.ForegroundColor = ConsoleColor.DarkYellow;

    foreach (FileEnvironment fileNotToBeModified in application.FilesNotToBeModified)
    {
        Console.WriteLine($" * {fileNotToBeModified.Path}");
    }

    Console.ForegroundColor = ConsoleColor.White;
}

Console.Write("Do you want to proceed modifications [Y/n] ");

string? res = Console.ReadLine();

switch (res?.ToLower())
{
    case "":
    case "y":
        break;
    case "n":
    default:
        Console.WriteLine("Exiting...");
        return 0;
}

Console.WriteLine("Modifying...");

application.ModifyFiles();

Console.WriteLine("All files have been modified");

return 0;