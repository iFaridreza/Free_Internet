namespace Free_Internet.Models;

internal class Vless : BaseConfig
{
    internal override IEnumerable<T> GetConfigRege<T>(string data)
    {
        try
        {
            string[] dataArray = data.Split('\n');
            const string pattern = @"^vless:\/\/(.*)";
            List<Vless> links = new();
            foreach (string link in dataArray)
            {
                var matchs = Regex.Match(link, pattern);
                if (matchs.Success is true)
                {
                    links.Add(new Vless
                    {
                        Link = matchs.Value,
                        ConfigType = ConfigType.Vless
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