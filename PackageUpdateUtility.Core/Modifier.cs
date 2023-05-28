namespace PackageUpdateUtility.Core;

public abstract class Modifier
{
    protected Modifier(string newValue)
    {
        NewValue = newValue;
    }

    public string NewValue { get; private set; }

    public abstract bool Verify(FileEnvironment fileEnvironment);
    
    public abstract void Modify(FileEnvironment fileEnvironment);

}