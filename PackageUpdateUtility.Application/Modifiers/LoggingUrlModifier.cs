using System.Xml;
using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Modifiers
{
    public class LoggingUrlModifier : Modifier
    {
        public LoggingUrlModifier(string newValue) : base(newValue)
        {
        }

        public override bool Verify(FileEnvironment fileEnvironment)
        {
            XmlDocument xmlDocument = new XmlDocument();

            fileEnvironment.LoaderWriter.Load(fileEnvironment, (loaderStream) =>
            {
                xmlDocument.Load(loaderStream);
            });

            XmlNodeList addNodeList = xmlDocument.GetElementsByTagName("add");

            foreach (XmlNode node in addNodeList)
            {

                XmlAttribute keyAttribute = node.Attributes["key"];
                XmlAttribute valueAttribute = node.Attributes["value"];

                // check if the node has the attributes we need
                if (keyAttribute == null || valueAttribute == null)
                {
                    continue;
                }

                if (keyAttribute.Value == "Log:Seq:Remote:Uri")
                {
                    if (valueAttribute.Value == NewValue)
                    {
                        return false;
                    }
                }
            }

            fileEnvironment.ParsedData = xmlDocument;

            return true;
        }

        public override void Modify(FileEnvironment fileEnvironment)
        {
            XmlDocument xmlDocument = (XmlDocument)fileEnvironment.ParsedData;

            XmlNodeList addNodeList = xmlDocument.GetElementsByTagName("add");

            foreach (XmlNode node in addNodeList)
            {

                XmlAttribute keyAttribute = node.Attributes["key"];
                XmlAttribute valueAttribute = node.Attributes["value"];

                // check if the node has the attributes we need
                if (keyAttribute == null || valueAttribute == null)
                {
                    continue;
                }

                if (keyAttribute.Value == "Log:Seq:Remote:Uri")
                {
                    valueAttribute.Value = NewValue;
                }
            }

            fileEnvironment.LoaderWriter.Write(fileEnvironment, (writeStream) =>
            {
                xmlDocument.Save(writeStream);
            });
        }
    }
}