namespace PackageUpdateUtility.Core;

public interface IWritable
{
    
    public Stream OpenWrite();

    public void Close();

}