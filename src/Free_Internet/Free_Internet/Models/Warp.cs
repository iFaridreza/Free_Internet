namespace Free_Internet.Models;

internal partial class Warp : BaseConfig<Warp>
{
    public override IEnumerable<T> GetConfigRegex<T>(string data) => 
        base.GetConfigRegex<T>(data, WarpRegex(), ConfigType.Warp);

    [GeneratedRegex(@"^warp:\/\/(.*)")]
    private static partial Regex WarpRegex();
}