namespace Free_Internet.Models;

internal partial class Hy2 : BaseConfig<Hy2>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data) =>
        base.GetConfigRegex<T>(data, Hy2Regex(), ConfigType.Hy2);

    [GeneratedRegex(@"hy2:\/\/(.*)")]
    private static partial Regex Hy2Regex();
}