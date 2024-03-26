using System.Text.Json;
using File = System.IO.File;

namespace Free_Internet.Services;
internal class ConfigProject
{
    internal string Token { get; set; } = null!;
    internal string UsernameChanellConfig { get; set; } = null!;
    internal string ChatIdChanellLog { get; set; } = null!;
    internal int ApiId { get; set; }
    internal string ApiHash { get; set; } = null!;
    internal string RepositoryName { get; set; } = null!;
    internal string RepositoryUrl { get; set; } = null!;
    internal string FileName { get; set; } = null!;
    internal string TextInlineButton { get; set; } = null!;
    internal string UrlInlineButton { get; set; } = null!;
    internal string PhoneNumber { get; set; } = null!;

    internal ConfigProject LoadConfig()
    {
        string _path = Path.Combine(Environment.CurrentDirectory, "Config.json");
        
        if (!File.Exists(_path))
        {
            JsonSerializerOptions optionJson = new() { WriteIndented = true };
            var objectsModel = new
            {
                Token,
                UsernameChanellConfig = "@",
                ChatIdChanellLog = "",
                ApiId,
                ApiHash,
                RepositoryName,
                RepositoryUrl,
                FileName,
                TextInlineButton,
                UrlInlineButton,
                PhoneNumber

            };
            var jsonSerilizeObj = JsonSerializer.Serialize(objectsModel, optionJson);
            File.WriteAllText(_path, jsonSerilizeObj);
        }

        string configFileData = File.ReadAllText(_path);

        var objectData = JsonSerializer.Deserialize<ConfigProject>(configFileData);
        return objectData ?? new ConfigProject();
    }
}
