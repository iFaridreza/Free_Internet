namespace Free_Internet.Models;

internal partial class ShadowSocks : BaseConfig<ShadowSocks>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data) =>
        base.GetConfigRegex<T>(data, ShadowSocksRegex(), ConfigType.ShadowSocks);


    [GeneratedRegex(@"^shadowsocks:\/\/(.*)")]
    private static partial Regex ShadowSocksRegex();
}