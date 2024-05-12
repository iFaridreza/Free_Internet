using File = System.IO.File;

namespace Free_Internet.Services;
internal sealed class ConfigManager
{
    private readonly string _urlRepository;
    private readonly string _repositoryPath;
    private readonly string _repositoryName;

    //todo: read this method in details
    public ConfigManager(string urlRepository, string repositoryName)
    {
        _urlRepository = urlRepository;
        
        string currentDir = Environment.CurrentDirectory;
        string folderRepo = "Repository/";
        
        _repositoryPath = Path.Combine(currentDir, folderRepo);
        
        if (!Directory.Exists(_repositoryPath))
        {
            Directory.CreateDirectory(_repositoryPath);
        }
        _repositoryName = repositoryName;
    }

    internal async Task<bool> DownloadRepositoryAsync()
    {
        string pathZip = $"{Path.Combine(_repositoryPath, _repositoryName)}.zip";
        
        if (File.Exists(pathZip))
        {
            File.Delete(pathZip);
        }
        using HttpClient httpClient = new();
        using var response = await httpClient.GetAsync(_urlRepository, HttpCompletionOption.ResponseContentRead);
        
        if (!response.IsSuccessStatusCode) return await Task.FromResult(false);

        //todo: must await then check if they are not dependent 
        //and use await Task.WhenAll()
        await using var contentStream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = new FileStream(pathZip, FileMode.Create);
        
        await contentStream.CopyToAsync(fileStream);
        return await Task.FromResult(true);
    }

    internal string UnzipRepository(string repositoryName)
    {
        string zipPath = $"{Path.Combine(_repositoryPath, _repositoryName)}.zip";
        
        var repositoryPaths = Directory.GetDirectories(_repositoryPath);
        if (repositoryPaths.Length > 0)
            foreach (var item in repositoryPaths)
                Directory.Delete(item, true);
        
        System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, _repositoryPath);
        File.Delete(zipPath);
        repositoryPaths = Directory.GetDirectories(_repositoryPath);
        
        return repositoryPaths[0];
    }

    internal static string GetDataFile(string pathFolder, string fileName)
    {
        var fileDir = Directory.GetFiles(pathFolder);
        fileDir = fileDir.Select(x =>
                x.Replace($"{pathFolder}/", ""))
            .Select(x=> x.Replace($"{pathFolder}\\", ""))
            .ToArray();
            
        if (!fileDir.Contains(fileName))
            return string.Empty;
            
        var pathFile = Path.Combine(pathFolder, fileName);
        var dataFile = File.ReadAllText(pathFile);
            
        return dataFile;
    }

    internal IEnumerable<T> GetLinkConfig<T>(string dataFile, T tConfig) where T : BaseConfig<T>, new() =>
        tConfig.GetConfigRegex<T>(dataFile).Reverse();
}
