using System.Xml;
using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Modifiers;

public class LoggingUrlModifier : Modifier
{

    private XmlDocument _document;
    
    public LoggingUrlModifier(string newValue) : base(newValue)
    {
        _document = new XmlDocument();
    }

    public override bool Verify(FileEnvironment fileEnvironment)
    {
        _document.LoadXml(fileEnvironment.Data);

        XmlNodeList addNodeList = _document.GetElementsByTagName("add");
        
        foreach (XmlNode node in addNodeList)
        {

            XmlAttribute? keyAttribute = node.Attributes?["key"];
            XmlAttribute? valueAttribute = node.Attributes?["value"];
            
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
        
        return true;
    }

    public override void Modify(FileEnvironment fileEnvironment)
    {
        XmlNodeList addNodeList = _document.GetElementsByTagName("add");
        
        foreach (XmlNode node in addNodeList)
        {

            XmlAttribute? keyAttribute = node.Attributes?["key"];
            XmlAttribute? valueAttribute = node.Attributes?["value"];
            
            // check if the node has the attributes we need
            if (keyAttribute == null || valueAttribute == null)
            {
                throw new Exception($"Cannot process modification on file {fileEnvironment.Path}");
            }
            
            if (keyAttribute.Value == "Log:Seq:Remote:Uri")
            {
                valueAttribute.Value = NewValue;
            }
        }
        
        _document.Save(fileEnvironment.WriteStream);
        fileEnvironment.WriteStream.Close();
    }
}