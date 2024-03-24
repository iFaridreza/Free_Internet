using System.Text.Json;
using TL.Methods;
using File = System.IO.File;

namespace Free_Internet.Services;
internal static class ConfigProject
{
    internal static string Token { get; set; } = null!;
    internal static string UsernameChanellConfig { get; set; } = null!;
    internal static int ApiId { get; set; }
    internal static string ApiHash { get; set; } = null!;
    internal static string RepositoryName { get; set; } = null!;
    internal static string RepositoryUrl { get; set; } = null!;
    internal static string FileName { get; set; } = null!;
    internal static string TextInlineButton { get; set; } = null!;
    internal static string UrlInlineButton { get; set; } = null!;
    internal static string PhoneNumber { get; set; } = null!;

    private static string _path;

    static ConfigProject()
    {
        _path = Path.Combine(Environment.CurrentDirectory, "Config.json");

        if (!File.Exists(_path))
        {
            JsonSerializerOptions optionJson = new() { WriteIndented = true };
            var objectsModel = new
            {
                Token,
                UsernameChanellConfig = "@",
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

        if (String.IsNullOrEmpty(configFileData)) return;

        var objectData = JsonSerializer.Deserialize<Dictionary<string, string>>(configFileData);
        if (objectData is null) return;

        Token = objectData["Token"];
        UsernameChanellConfig = objectData["UsernameChanellConfig"];
        ApiId = int.TryParse(objectData["ApiId"], out int apiId) ? apiId : default;
        ApiHash = objectData["ApiHash"];
        RepositoryName = objectData["RepositoryName"];
        RepositoryUrl = objectData["RepositoryUrl"];
        TextInlineButton = objectData["TextInlineButton"];
        UrlInlineButton = objectData["UrlInlineButton"];
        FileName = objectData["FileName"];
        PhoneNumber = objectData["PhoneNumber"].Replace(" ","");
    }
}
