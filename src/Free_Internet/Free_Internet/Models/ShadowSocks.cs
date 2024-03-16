namespace Free_Internet.Models;

internal class ShadowSocks : BaseConfig
{
    internal override IEnumerable<T> GetConfigRege<T>(string data)
    {
        try
        {
            string[] dataArray = data.Split('\n');
            const string pattern = @"^shadowsocks:\/\/(.*)";
            List<ShadowSocks> links = new();
            foreach (string link in dataArray)
            {
                var matchs = Regex.Match(link, pattern);
                if (matchs.Success is true)
                {
                    links.Add(new ShadowSocks
                    {
                        Link = matchs.Value,
                        ConfigType = ConfigType.ShadowSocks
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