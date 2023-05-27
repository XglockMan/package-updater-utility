namespace Core;

public abstract class Modifier
{
    protected Modifier(string newValue)
    {
        NewValue = newValue;
    }

    public string NewValue { get; private set; }
    
    

}