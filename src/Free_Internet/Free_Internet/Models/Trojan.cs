namespace Free_Internet.Models;

internal class Trojan : BaseConfig<Trojan>
{
    internal override sealed IEnumerable<Trojan> GetConfigRegex(string data)
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