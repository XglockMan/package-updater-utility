using PackageUpdateUtility.Core.FileLoaders;

namespace PackageUpdateUtility.Core;

public class Application
{

    private List<FileEnvironment> _filesToLoad;

    private List<FileEnvironment> _loadedFiles;

    public List<FileEnvironment> FilesToBeModified { get; private set; }

    public List<FileEnvironment> FilesNotToBeModified { get; private set; }

    private Dictionary<Type, ILoaderWriter> _fileLoaderWriters;
    private Dictionary<Type, Modifier> _modifiers;
    
    public Application()
    {
        _filesToLoad = new List<FileEnvironment>();
        FilesToBeModified = new List<FileEnvironment>();
        FilesNotToBeModified = new List<FileEnvironment>();
        _fileLoaderWriters = new Dictionary<Type, ILoaderWriter>();
        _modifiers = new Dictionary<Type, Modifier>();
        _loadedFiles = new List<FileEnvironment>();
    }

    public static Application InitWithBasics()
    {
        Application application = new Application();
        
        application.RegisterFileLoader<ZipFileLoaderWriter>();
        application.RegisterFileLoader<BasicFileLoaderWriter>();

        return application;
    }

    public ILoaderWriter GetLoaderWriter<T>() where T: ILoaderWriter
    {
        lock (_fileLoaderWriters)
        {
            return _fileLoaderWriters[typeof(T)];
        }
    }

    public Modifier GetModifier<T>() where T: Modifier
    {
        lock (_modifiers)
        {
            return _modifiers[typeof(T)];
        }
    }

    public void RegisterFileLoader<T>() where T: ILoaderWriter
    {

        lock (_fileLoaderWriters)
        {
            if (_fileLoaderWriters.ContainsKey(typeof(T)))
            {
                throw new Exception("This type of file loader is already been injected");
            }
            
            T instance = Activator.CreateInstance<T>();
            
            _fileLoaderWriters.Add(typeof(T), instance);
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

    public void RegisterFile<TL, TM>(string path) where TL : ILoaderWriter where TM: Modifier
    {

        ILoaderWriter loaderWriter = GetLoaderWriter<TL>();
        Modifier modifier = GetModifier<TM>();

        FileEnvironment fileEnvironment = new FileEnvironment(this, path, modifier, loaderWriter);
        
        _filesToLoad.Add(fileEnvironment);
    }

    public void LoadFiles()
    {

        foreach (FileEnvironment file in _filesToLoad)
        {
            _loadedFiles.Add(file);
        }
        
    }


    public void VerifyFiles()
    {

        foreach (FileEnvironment loadedFile in _loadedFiles)
        {

            try
            {
                if (loadedFile.Modifier.Verify(loadedFile))
                {
                    FilesToBeModified.Add(loadedFile);
                    continue;
                }
            
                FilesNotToBeModified.Add(loadedFile);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught while verifying file {loadedFile.Path} with {e.Message}");
                throw;
            }
        }
        
    }

    public void ModifyFiles()
    {

        foreach (FileEnvironment fileToBeModified in FilesToBeModified)
        {
            try
            {
                fileToBeModified.Modifier.Modify(fileToBeModified);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught while modifying file {fileToBeModified.Path} with {e.Message}");
                throw;
            }
        }
        
    }
    
    
    
    


}