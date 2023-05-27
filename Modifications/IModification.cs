namespace PackageUpdateUtility.Modifications;

public interface IModification
{
    
    public delegate Stream OpenFile(string path);
    
    public string Name { get; set; }
    public string FilePath { get; set; }

    public OpenFile OpenFileDelegate { get; set; }
    
    public void RunModification();



}