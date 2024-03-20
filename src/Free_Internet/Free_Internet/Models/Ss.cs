namespace Free_Internet.Models;

internal partial class Ss : BaseConfig<Ss>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data) => 
        base.GetConfigRegex<T>(data, SsRegex(), ConfigType.Ss);

    [GeneratedRegex(@"^ss:\/\/(.*)")]
    private static partial Regex SsRegex();
}