// See https://aka.ms/new-console-template for more information

using PackageUpdateUtility.Core;
using PackageUpdateUtility.Core.FileLoaders;
using PackageUpdateUtility.Modifiers;

Application application = Application.InitWithBasics();

application.RegisterModifier<LoggingUrlModifier>("https://seq-log.eposid.eu/");
application.RegisterModifier<VersionNumberModifier>("[old]-A");

application.RegisterFile<BasicFileLoader, LoggingUrlModifier>("HaSaM/Epos.Server/Services/Windows/Epos/LocalConfiguration/AppSettings.config");
application.RegisterFile<ZipFileLoader, LoggingUrlModifier>("HaSaM/Epos.Server/Services/IIS/UpdateWeb/Packages/Epos/Epos.zip/LocalConfiguration/AppSettings.config");
application.RegisterFile<BasicFileLoader, VersionNumberModifier>("HaSaM/Epos.Server/Services/IIS/UpdateWeb/Packages/Epos/package.xml");

application.LoadFiles();

application.VerifyFiles();

Console.WriteLine();

Console.ForegroundColor = ConsoleColor.Green;

foreach (FileEnvironment fileToByModified in application.FilesToBeModified)
{
    Console.WriteLine($" + {fileToByModified.Path}");
}

Console.WriteLine();

Console.ForegroundColor = ConsoleColor.DarkYellow;

foreach (FileEnvironment fileNotToBeModified in application.FilesNotToBeModified)
{
    Console.WriteLine($" * {fileNotToBeModified.Path}");
}

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine();

Console.Write("Do you want to proceed modifications [Y/n] ");

string? res = Console.ReadLine();

switch (res?.ToLower())
{
    case "n":
        return 0;
    case "y":
        break;
    
    default:
        return 0;
}

Console.WriteLine("Modifying...");

application.ModifyFiles();

Console.WriteLine("All files have been modified");

return 0;