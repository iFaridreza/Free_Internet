namespace Free_Internet.Models;

internal partial class Vless : BaseConfig<Vless>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data)
    {
        return base.GetConfigRegex<T>(data, VlessRegex(), ConfigType.Vless);
    }

    [GeneratedRegex(@"^vless:\/\/(.*)")]
    private static partial Regex VlessRegex();
}