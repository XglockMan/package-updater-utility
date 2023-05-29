using PackageUpdateUtility.Core.FileLoaders;

namespace PackageUpdateUtility.Core;

public class Application
{

    private List<FileEnvironment> _filesToLoad;

    private List<FileEnvironment> _loadedFiles;

    public List<FileEnvironment> FilesToBeModified { get; private set; }

    public List<FileEnvironment> FilesNotToBeModified { get; private set; }

    private Dictionary<Type, FileLoader> _fileLoaders;
    private Dictionary<Type, Modifier> _modifiers;
    
    public Application()
    {
        _filesToLoad = new List<FileEnvironment>();
        FilesToBeModified = new List<FileEnvironment>();
        FilesNotToBeModified = new List<FileEnvironment>();
        _fileLoaders = new Dictionary<Type, FileLoader>();
        _modifiers = new Dictionary<Type, Modifier>();
        _loadedFiles = new List<FileEnvironment>();
    }

    public static Application InitWithBasics()
    {
        Application application = new Application();
        
        application.RegisterFileLoader<ZipFileLoader>();
        application.RegisterFileLoader<BasicFileLoader>();

        return application;
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

    public void RegisterFile<TL, TM>(string path) where TL : FileLoader where TM: Modifier
    {

        FileLoader loader = GetLoader<TL>();
        Modifier modifier = GetModifier<TM>();

        FileEnvironment fileEnvironment = new FileEnvironment(this, path, modifier, loader);
        
        _filesToLoad.Add(fileEnvironment);
    }

    public void LoadFiles()
    {

        foreach (FileEnvironment file in _filesToLoad)
        {
            FileEnvironment loadedFile = file.FileLoader.Load(file);
            
            _loadedFiles.Add(loadedFile);
            
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