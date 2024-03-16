namespace Free_Internet.Models;

internal class Warp : BaseConfig
{
    internal override IEnumerable<T> GetConfigRege<T>(string data)
    {
        try
        {
            string[] dataArray = data.Split('\n');
            const string pattern = @"^warp:\/\/(.*)";
            List<Warp> links = new();
            foreach (string link in dataArray)
            {
                var matchs = Regex.Match(link, pattern);
                if (matchs.Success is true)
                {
                    links.Add(new Warp
                    {
                        Link = matchs.Value,
                        ConfigType = ConfigType.Warp
                    });
                }
            }
            if (links.Count > 0)
            {
                links = links.Distinct().ToList();
            }
            return (IEnumerable<T>)links;
        }
        catch
        {
            throw;
        }
    }
}