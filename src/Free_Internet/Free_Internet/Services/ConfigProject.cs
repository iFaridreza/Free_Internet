using System.Text.Json;
using File = System.IO.File;

namespace Free_Internet.Services;
internal class ConfigProject
{
    public string Token { get; set; } = null!;
    public string UsernameChanellConfig { get; set; } = null!;
    public string ChatIdChanellLog { get; set; } = null!;
    public int ApiId { get; set; }
    public string ApiHash { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string RepositoryUrl { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string TextInlineButton { get; set; } = null!;
    public string UrlInlineButton { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

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
