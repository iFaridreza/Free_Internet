namespace Free_Internet.Models;

internal partial class Vmess : BaseConfig<Vmess>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data) => 
        base.GetConfigRegex<T>(data, VmessRegex(), ConfigType.Vmess);

    [GeneratedRegex(@"^vmess:\/\/(.*)")]
    private static partial Regex VmessRegex();
}