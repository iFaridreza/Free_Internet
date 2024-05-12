namespace Free_Internet.Models;

internal partial class Trojan : BaseConfig<Trojan>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data) => 
        base.GetConfigRegex<T>(data, TrojanRegex(), ConfigType.Trojan);

    [GeneratedRegex(@"^trojan:\/\/(.*)")]
    private static partial Regex TrojanRegex();
}