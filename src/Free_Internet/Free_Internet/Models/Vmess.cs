namespace Free_Internet.Models;

internal class Vmess : BaseConfig
{
    internal override IEnumerable<T> GetConfigRege<T>(string data)
    {
        try
        {
            string[] dataArray = data.Split('\n');
            const string pattern = @"^vmess:\/\/(.*)";
            List<Vmess> links = new();
            foreach (string link in dataArray)
            {
                var matchs = Regex.Match(link, pattern);
                if (matchs.Success is true)
                {
                    links.Add(new Vmess
                    {
                        Link = matchs.Value,
                        ConfigType = ConfigType.Vmess
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