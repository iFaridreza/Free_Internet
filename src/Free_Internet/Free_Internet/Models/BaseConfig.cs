namespace Free_Internet.Models;
internal abstract class BaseConfig<T> where T : class, new() 
{
    internal string Link { get; set; } = null!;
    internal abstract IEnumerable<T> GetConfigRegex(string data) ;
}