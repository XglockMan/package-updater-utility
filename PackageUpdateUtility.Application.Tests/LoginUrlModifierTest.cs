using System.Xml;
using PackageUpdateUtility.Core;
using PackageUpdateUtility.Modifiers;

namespace PackageUpdateUtility.Tests;

public class LoginUrlModifierTest
{

    private string _testData =
        "<appSettings><add key=\"Url\" value=\"http://EPOS02:9006/\" /><add key=\"Language\" value=\"cs\" /><add key=\"Log:Seq:Remote:Uri\" value=\"https://hasam-logging.azurewebsites.net\" /><add key=\"Log:Seq:Remote:ApiKey\" value=\"pqvOSIRslttkZXtB2C86\" /></appSettings>";

    public class TestFileLoader : FileLoader
    {
        
        public MemoryStream TestDataStream = new();
        
        public override FileEnvironment Load(FileEnvironment fileEnvironment)
        {
            fileEnvironment.Data = "<appSettings><add key=\"Url\" value=\"http://EPOS02:9006/\" /><add key=\"Language\" value=\"cs\" /><add key=\"Log:Seq:Remote:Uri\" value=\"https://hasam-logging.azurewebsites.net\" /><add key=\"Log:Seq:Remote:ApiKey\" value=\"pqvOSIRslttkZXtB2C86\" /></appSettings>";

            fileEnvironment.WriteStream = TestDataStream;
            
            return fileEnvironment;
        }
        
    }

    private Application _application;
    
    [SetUp]
    public void SetUp()
    {
        _application = new Application();
        
        _application.RegisterFileLoader<TestFileLoader>();
        
        _application.RegisterModifier<LoggingUrlModifier>("TestVal");
        
        _application.RegisterFile<TestFileLoader, LoggingUrlModifier>("TestFile");
    }

    [Test]
    public void ModifyTest()
    {
        _application.LoadFiles();
        
        _application.VerifyFiles();

        FileEnvironment fileEnvironment = _application.FilesToBeModified[0];
        
        Assert.NotNull(fileEnvironment);
        
        _application.ModifyFiles();

        TestFileLoader fileLoader = (TestFileLoader) _application.GetLoader<TestFileLoader>();

        string dataString = System.Text.Encoding.UTF8.GetString(fileLoader.TestDataStream.ToArray());

        XmlDocument document = new XmlDocument();
        
        document.LoadXml(dataString);
        
        Assert.NotNull(document);

        XmlNodeList nodeList = document.GetElementsByTagName("add");

        XmlNode node = nodeList[2];
        
        Assert.NotNull(node);
        
        Assert.That(node.Attributes["value"]!.Value, Is.EqualTo("TestVal"));

    }
    
    
    
    
}