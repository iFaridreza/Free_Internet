﻿namespace Free_Internet.Models;
internal abstract class BaseConfig
{
    internal string Link { get; set; } = null!;
    internal ConfigType ConfigType { get; set; }
    internal abstract IEnumerable<T> GetConfigRege<T>(string data) where T : class, new();
}