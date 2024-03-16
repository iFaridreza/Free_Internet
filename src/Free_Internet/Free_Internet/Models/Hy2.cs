
namespace Free_Internet.Models;

internal class Hy2 : BaseConfig
{
    internal override IEnumerable<T> GetConfigRege<T>(string data)
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
                        Link = matchs.Value,
                        ConfigType = ConfigType.Hy2
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