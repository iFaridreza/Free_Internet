namespace Free_Internet.Services;

internal sealed partial class ConfigParser
{
    public IEnumerable<Config> Parse(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            yield break;

        foreach (var rawLine in data.Split(Environment.NewLine))
        {
            var line = rawLine.AsSpan().Trim();
            if (line.Length == 0) continue;

            if (TryParseLine(line, out var entry) && entry is not null)
                yield return entry;
        }
    }

    public bool TryParseLine(ReadOnlySpan<char> line, out Config? entry)
    {
        if (VmessRegex().IsMatch(line))        { entry = new Config(line.ToString(), ConfigType.Vmess); return true; }
        if (VlessRegex().IsMatch(line))        { entry = new Config(line.ToString(), ConfigType.Vless); return true; }
        if (TrojanRegex().IsMatch(line))       { entry = new Config(line.ToString(), ConfigType.Trojan); return true; }
        if (Hy2Regex().IsMatch(line))          { entry = new Config(line.ToString(), ConfigType.Hy2); return true; }
        if (ShadowSocksRegex().IsMatch(line))  { entry = new Config(line.ToString(), ConfigType.ShadowSocks); return true; }
        if (SsRegex().IsMatch(line))           { entry = new Config(line.ToString(), ConfigType.Ss); return true; }
        if (TuicRegex().IsMatch(line))         { entry = new Config(line.ToString(), ConfigType.Tuic); return true; }
        if (WarpRegex().IsMatch(line))         { entry = new Config(line.ToString(), ConfigType.Warp); return true; }

        entry = null;
        return false;
    }

    [GeneratedRegex(@"^vmess:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex VmessRegex();

    [GeneratedRegex(@"^vless:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex VlessRegex();

    [GeneratedRegex(@"^trojan:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex TrojanRegex();

    [GeneratedRegex(@"^hy2:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex Hy2Regex();

    [GeneratedRegex(@"^shadowsocks:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ShadowSocksRegex();

    [GeneratedRegex(@"^ss:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SsRegex();

    [GeneratedRegex(@"^tuic:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex TuicRegex();

    [GeneratedRegex(@"^warp:\/\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex WarpRegex();
}