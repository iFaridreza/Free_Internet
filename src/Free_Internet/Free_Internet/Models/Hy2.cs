
namespace Free_Internet.Models;

internal class Hy2 : BaseConfig<Hy2>
{
    internal override sealed IEnumerable<Hy2> GetConfigRegex(string data)
    {
        try
        {
            string[] dataArray = data.Split('\n');
            const string pattern = @"hy2:\/\/(.*)";
            List<Hy2> links = new();
            foreach (string link in dataArray)
            {
                var matchs = Regex.Match(link, pattern);
                if (matchs.Success is true)
                {
                    links.Add(new Hy2
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