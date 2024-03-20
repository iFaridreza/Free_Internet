namespace Free_Internet.Models;
internal abstract class BaseConfig<TClass> : IConfig where TClass: BaseConfig<TClass>, new() 
{
    public string Link { get; init; } = null!;
    public ConfigType ConfigType { get; init; }

    public abstract IEnumerable<T> GetConfigRegex<T>(string data) where T : class, new();
    
    //todo: make virtual if needed?
    protected IEnumerable<T> GetConfigRegex<T>(string data, Regex regex, ConfigType configType)
        where T : class, new()
    {
        //todo: ask if data use linux new line char? or /t/n?
        string[] dataArray = data.Split('\n');
        
        var links = dataArray.Select(link => regex.Match(link))
            .Where(matches => matches.Success)
            .Select(matches => new TClass { Link = matches.Value, ConfigType = configType }).ToList();

        if (links.Count > 0)
            links = links.Distinct().ToList();
        
        return (IEnumerable<T>)links;
    }
}