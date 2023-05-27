namespace Core;

public class Application
{

    private List<IFileEnvironment> _loadedFiles;
    
    private Dictionary<Type, FileLoader> _fileLoaders;
    private Dictionary<Type, Modifier> _modifiers;
    
    public Application()
    {
        _fileLoaders = new Dictionary<Type, FileLoader>();
        _modifiers = new Dictionary<Type, Modifier>();
        _loadedFiles = new List<IFileEnvironment>();
    }

    public FileLoader GetLoader<T>() where T: FileLoader
    {
        lock (_fileLoaders)
        {
            return _fileLoaders[typeof(T)];
        }
    }

    public Modifier GetModifier<T>() where T: Modifier
    {
        lock (_modifiers)
        {
            return _modifiers[typeof(T)];
        }
    }

    public void RegisterFileLoader<T>() where T: FileLoader
    {

        lock (_fileLoaders)
        {
            if (_fileLoaders.ContainsKey(typeof(T)))
            {
                throw new Exception("This type of file loader is already been injected");
            }
            
            T instance = Activator.CreateInstance<T>();
            
            _fileLoaders.Add(typeof(T), instance);
        }
        
    }

    public void RegisterModifier<T>(string value) where T: Modifier
    {

        lock (_modifiers)
        {
            if (_modifiers.ContainsKey(typeof(T)))
            {
                throw new Exception("This type of file loader is already been injected");
            }

            object[] args = {
                value
            };
            
            T instance = (T) Activator.CreateInstance(typeof(T), args);
            
            _modifiers.Add(typeof(T), instance);
        }
        

    }


}