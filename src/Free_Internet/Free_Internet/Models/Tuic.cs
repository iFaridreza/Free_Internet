namespace Free_Internet.Models;

internal partial class Tuic : BaseConfig<Tuic>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data) => 
        base.GetConfigRegex<T>(data, TuicRegex(), ConfigType.Tuic);

    [GeneratedRegex(@"^tuic:\/\/(.*)")]
    private static partial Regex TuicRegex();
}