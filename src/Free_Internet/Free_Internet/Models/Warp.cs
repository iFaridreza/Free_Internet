namespace Free_Internet.Models;

internal class Warp : BaseConfig<Warp>
{
    internal override sealed IEnumerable<Warp> GetConfigRegex(string data)
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