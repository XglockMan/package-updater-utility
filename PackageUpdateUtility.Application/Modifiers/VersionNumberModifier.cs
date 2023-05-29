using System.Xml;
using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Modifiers;

public class VersionNumberModifier : Modifier
{

    private XmlDocument _document;
    
    public VersionNumberModifier(string newValue) : base(newValue)
    {
        _document = new XmlDocument();
    }

    public override bool Verify(FileEnvironment fileEnvironment)
    {
        _document.LoadXml(fileEnvironment.Data);
        
        XmlNodeList elementsByTagName = _document.GetElementsByTagName("Version");
        
        XmlNode? verisonNode = elementsByTagName[0];
        
        // check if the node has the attributes we need
        if (verisonNode == null)
        {
            throw new Exception($"Cannot process verification on file {fileEnvironment.Path}");
        }

        if (verisonNode.InnerText == NewValue)
        {
            return false;
        }

        return true;
    }

    public override void Modify(FileEnvironment fileEnvironment)
    {
        
        _document.LoadXml(fileEnvironment.Data);
        
        XmlNodeList elementsByTagName = _document.GetElementsByTagName("Version");
        
        XmlNode? verisonNode = elementsByTagName[0];
        
        // check if the node has the attributes we need
        if (verisonNode == null)
        {
            throw new Exception($"Cannot process verification on file {fileEnvironment.Path}");
        }
        
        verisonNode.InnerText = NewValue.Replace("[old]", verisonNode.InnerText);
        
        _document.Save(fileEnvironment.WriteStream);
        fileEnvironment.WriteStream.Close();

    }
}