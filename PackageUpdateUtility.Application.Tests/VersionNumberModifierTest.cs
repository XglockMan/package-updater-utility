using System.Xml;

using NUnit.Framework;

using PackageUpdateUtility.Core;
using PackageUpdateUtility.Modifiers;

namespace PackageUpdateUtility.Tests
{
    public class VersionNumberModifierTest
    {
        private PackageUpdateUtility.Core.Application _application;

        [SetUp]
        public void SetUp()
        {
            _application = new PackageUpdateUtility.Core.Application();

            _application.RegisterFileLoader<TestFileLoader>();

            _application.RegisterModifier<VersionNumberModifier>("[old]-A");

            _application.RegisterFile<TestFileLoader, VersionNumberModifier>("<root><Version>10.2023.0517.1-dev</Version><Content><Assembly file=\"Epos.zip\"/></Content></root>");
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

            XmlNodeList nodeList = document.GetElementsByTagName("Version");

            XmlNode node = nodeList[0];

            Assert.NotNull(node);

            Assert.That(node.InnerText, Is.EqualTo("10.2023.0517.1-dev-A"));
        }
    }
}