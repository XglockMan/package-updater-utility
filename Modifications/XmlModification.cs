using System.Xml;

namespace PackageUpdateUtility.Modifications;

public abstract class XmlModification : IModification
{

    protected string NewValue;
    
    public XmlModification(string newNewValue, string filePath, IModification.OpenFile openFileDelegate)
    {
        NewValue = newNewValue;
        FilePath = filePath;
        OpenFileDelegate = openFileDelegate;
    }

    public abstract string Name { get; set; }
    public string FilePath { get; set; }

    protected Stream Stream { get; private set; }
    
    public IModification.OpenFile OpenFileDelegate { get; set; }
    
    public void RunModification()
    {
        Stream targetStream = OpenFileDelegate(FilePath);
        Stream = targetStream;
        XmlDocument document = new XmlDocument();
        document.Load(targetStream);
        RunXmlModification(document);
    }
    
    public abstract void RunXmlModification(XmlDocument document);
}