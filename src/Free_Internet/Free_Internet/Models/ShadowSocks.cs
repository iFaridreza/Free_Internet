namespace Free_Internet.Models;

internal class ShadowSocks : BaseConfig<ShadowSocks>
{
    internal override sealed IEnumerable<ShadowSocks> GetConfigRegex(string data)
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
                        Link = matchs.Value
                    });
                }
            }
            if (links.Count > 0)
            {
                links = links.Distinct().ToList();
            }
            return links;
        }
        catch
        {
            throw;
        }
    }
}