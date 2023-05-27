using System.Xml;

namespace PackageUpdateUtility.Modifications;

public class UpdateConfigModification : XmlModification
{
    public UpdateConfigModification(string newNewValue, string filePath, IModification.OpenFile openFileDelegate) :
        base(newNewValue, filePath, openFileDelegate)
    {
    }

    public override string Name { get; set; } = "UpdateConfigModification";
    
    public override void RunXmlModification(XmlDocument document)
    {
        XmlNodeList elementsByTagName = document.GetElementsByTagName("Version");
        
        XmlNode? verisonNode = elementsByTagName[0];
        
        // check if the node has the attributes we need
        if (verisonNode == null)
        {
            throw new Exception($"Cannot process modification {Name} on file {FilePath}");
        }
        
        string oldData = verisonNode.InnerText;
        
        NewValue = NewValue.Replace("[old]", oldData);

        verisonNode.InnerText = NewValue;
        
        document.Save(FilePath);
    }
}