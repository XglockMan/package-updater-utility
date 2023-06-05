using System.Xml;

using NUnit.Framework;

using PackageUpdateUtility.Core;
using PackageUpdateUtility.Modifiers;

namespace PackageUpdateUtility.Tests
{
    public class LoginUrlModifierTest
    {

        private string _testData =
            "<appSettings><add key=\"Url\" value=\"http://EPOS02:9006/\" /><add key=\"Language\" value=\"cs\" /><add key=\"Log:Seq:Remote:Uri\" value=\"https://hasam-logging.azurewebsites.net\" /><add key=\"Log:Seq:Remote:ApiKey\" value=\"pqvOSIRslttkZXtB2C86\" /></appSettings>";

        private PackageUpdateUtility.Core.Application _application;

        [SetUp]
        public void SetUp()
        {
            _application = new PackageUpdateUtility.Core.Application();

            _application.RegisterFileLoader<TestFileLoader>();

            _application.RegisterModifier<LoggingUrlModifier>("TestVal");

            _application.RegisterFile<TestFileLoader, LoggingUrlModifier>(_testData);
        }

        [Test]
        public void ModifyTest()
        {
            _application.LoadFiles();

            _application.VerifyFiles();

            FileEnvironment fileEnvironment = _application.FilesToBeModified[0];

            Assert.NotNull(fileEnvironment);

            _application.ModifyFiles();

            TestFileLoader fileLoader = (TestFileLoader)_application.GetLoaderWriter<TestFileLoader>();

            string dataString = System.Text.Encoding.UTF8.GetString(fileLoader.MemoryStream.ToArray());

            XmlDocument document = new XmlDocument();

            document.LoadXml(dataString);

            Assert.NotNull(document);

            XmlNodeList nodeList = document.GetElementsByTagName("add");

            XmlNode node = nodeList[2];

            Assert.NotNull(node);

            Assert.That(node.Attributes["value"].Value, Is.EqualTo("TestVal"));

        }
    }
}