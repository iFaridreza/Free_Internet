namespace Free_Internet.Models;

internal class Vmess : BaseConfig<Vmess>
{
    internal override sealed IEnumerable<Vmess> GetConfigRegex(string data)
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