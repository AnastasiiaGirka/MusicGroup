using MusicGroup.Api.Db.Context.Entities.Base;
using MusicGroup.Common;
using Newtonsoft.Json;

namespace MusicGroup.Api.Db.FileDb;

public class FileDb
{
    private readonly string _directory;
    public FileDb()
    {
        _directory = $"{AppDomain.CurrentDomain.BaseDirectory}/FileDbStorage";    
        
        Initialize();
    }

    private void Initialize()
    {
        BaseDbEntity?[] dbEntities = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(BaseDbEntity)))
            .Select(type => Activator.CreateInstance(type) as BaseDbEntity)
            .ToArray();

        string[] fileNames = dbEntities.Select(item => item.GetType().Name).ToArray();

        var directory = Directory.CreateDirectory(_directory);

        string directoryFullPath = directory.FullName;
        
        foreach (string fileName in fileNames)
        {
            string filePath = $"{directoryFullPath}/{fileName}";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
        }
    }

    public void Insert<T>(T entity) where T: BaseDbEntity
    {
        string filePath = ResolveFilePath<T>();

        try
        {
            File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
        }

        string content = File.ReadAllText(filePath);
        
        var list = JsonConvert.DeserializeObject<List<T>>(content) ?? new List<T>();
        
        list.Add(entity);
        
        var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);

        File.WriteAllText(filePath, convertedJson);
    }

    public T[] List<T>() where T : BaseDbEntity
    {
        string filePath = ResolveFilePath<T>();
        
        T[]? items = JsonConvert.DeserializeObject<T[]>(File.ReadAllText(filePath));
        
        return items;
    }

    private string ResolveFilePath<T>() where T : BaseDbEntity
    {
        string fileName = typeof(T).Name;
            
        string filePath = $"{_directory}/{fileName}";

        return filePath;
    }
}