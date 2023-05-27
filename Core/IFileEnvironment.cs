namespace Core;

public interface IFileEnvironment
{
    
    public Stream WriteStream { get; set; }
    
    public string Data { get; set; }
    
    public Type Modificator { get; protected set; }

}