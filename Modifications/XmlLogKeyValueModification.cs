using System.Xml;

namespace PackageUpdateUtility.Modifications;

public class LocalConfigurationModification : XmlModification
{

    public LocalConfigurationModification(string newLoggingUrl, string filePath, IModification.OpenFile openFile) :
        base(newLoggingUrl, filePath, openFile)
    {
    }
    
    public override string Name { get; set; } = "LocalConfigurationModification";

    public override void RunXmlModification(XmlDocument document)
    {

        XmlNodeList addNodeList = document.GetElementsByTagName("add");

        foreach (XmlNode node in addNodeList)
        {

            XmlAttribute? keyAttribute = node.Attributes?["key"];
            XmlAttribute? valueAttribute = node.Attributes?["value"];
            
            // check if the node has the attributes we need
            if (keyAttribute == null || valueAttribute == null)
            {
                throw new Exception($"Cannot process modification {Name} on file {FilePath}");
            }
            
            if (keyAttribute.Value == "Log:Seq:Remote:Uri")
            {
                valueAttribute.Value = NewValue;
            }
        }
        
        document.Save(FilePath);

    }
}