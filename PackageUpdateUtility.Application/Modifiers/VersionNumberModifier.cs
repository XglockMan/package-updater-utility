using System.Xml;
using PackageUpdateUtility.Core;

namespace PackageUpdateUtility.Modifiers;

public class VersionNumberModifier : Modifier
{

    public VersionNumberModifier(string newValue) : base(newValue)
    {
    }

    public override bool Verify(FileEnvironment fileEnvironment)
    {
        XmlDocument xmlDocument = new XmlDocument();
        
        xmlDocument.LoadXml(fileEnvironment.Data);

        XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Version");
        
        XmlNode? verisonNode = elementsByTagName[0];
        
        // check if the node has the attributes we need
        if (verisonNode == null)
        {
            throw new Exception($"Cannot process verification on file {fileEnvironment.Path}");
        }
        
        if (verisonNode.InnerText.EndsWith(NewValue))
        {
            return false;
        }

        return true;
    }

    public override void Modify(FileEnvironment fileEnvironment)
    {

        XmlDocument xmlDocument = new XmlDocument();
        
        xmlDocument.LoadXml(fileEnvironment.Data);
        
        XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Version");
        
        XmlNode? verisonNode = elementsByTagName[0];
        
        // check if the node has the attributes we need
        if (verisonNode == null)
        {
            throw new Exception($"Cannot process verification on file {fileEnvironment.Path}");
        }
        
        verisonNode.InnerText = NewValue.Replace("[old]", verisonNode.InnerText);

        IWritable fileWritable = fileEnvironment.FileWritable;
        Stream writeStream = fileWritable.OpenWrite();
        
        xmlDocument.Save(writeStream);
        
        fileWritable.Close();

    }
}