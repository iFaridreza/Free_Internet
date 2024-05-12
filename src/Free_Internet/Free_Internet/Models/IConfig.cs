namespace Free_Internet.Models;

internal interface IConfig
{
    string Link { get; init; }
    ConfigType ConfigType { get; init; }
}