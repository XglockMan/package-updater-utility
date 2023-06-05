using System;
using System.Xml;

using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Modifiers
{
    public class VersionNumberModifier : Modifier
    {

        public VersionNumberModifier(string newValue) : base(newValue)
        {
        }

        public override bool Verify(FileEnvironment fileEnvironment)
        {
            XmlDocument xmlDocument = new XmlDocument();

            fileEnvironment.LoaderWriter.Load(fileEnvironment, (loaderStream) =>
            {
                xmlDocument.Load(loaderStream);
            });

            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Version");

            XmlNode verisonNode = elementsByTagName[0];

            // check if the node has the attributes we need
            if (verisonNode == null)
            {
                throw new Exception($"Cannot process verification on file {fileEnvironment.Path}");
            }

            if (verisonNode.InnerText.EndsWith(NewValue))
            {
                return false;
            }

            fileEnvironment.ParsedData = xmlDocument;

            return true;
        }

        public override void Modify(FileEnvironment fileEnvironment)
        {

            XmlDocument xmlDocument = (XmlDocument)fileEnvironment.ParsedData;

            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Version");

            XmlNode verisonNode = elementsByTagName[0];

            // check if the node has the attributes we need
            if (verisonNode == null)
            {
                throw new Exception($"Cannot process verification on file {fileEnvironment.Path}");
            }

            verisonNode.InnerText += NewValue;

            fileEnvironment.LoaderWriter.Write(fileEnvironment, (writeStream) =>
            {
                xmlDocument.Save(writeStream);
            });
        }
    }
}