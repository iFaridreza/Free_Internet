namespace Free_Internet.Models;

internal class Trojan : BaseConfig
{
    internal override IEnumerable<T> GetConfigRege<T>(string data)
    {
        try
        {
            string[] dataArray = data.Split('\n');
            const string pattern = @"^trojan:\/\/(.*)";
            List<Trojan> links = new();
            foreach (string link in dataArray)
            {
                var matchs = Regex.Match(link, pattern);
                if (matchs.Success is true)
                {
                    links.Add(new Trojan
                    {
                        Link = matchs.Value,
                        ConfigType = ConfigType.Trojan
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