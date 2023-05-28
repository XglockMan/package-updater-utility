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

foreach (FileEnvironment fileToByModified in application.FilesToBeModified)
{
    Console.WriteLine(fileToByModified.Path);
}

application.ModifyFiles();