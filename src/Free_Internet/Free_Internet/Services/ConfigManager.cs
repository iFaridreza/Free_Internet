using File = System.IO.File;

namespace Free_Internet.Services;
internal sealed class ConfigManager
{
    private string _urlRepository;
    private string _repositoryPath;
    private string _repositoryName;

    public ConfigManager(string urlRepository, string repositoryName)
    {
        _urlRepository = urlRepository;
        string curentDir = Environment.CurrentDirectory;
        string folderRepo = "Repository/";
        _repositoryPath = Path.Combine(curentDir, folderRepo);
        if (!Directory.Exists(_repositoryPath))
        {
            Directory.CreateDirectory(_repositoryPath);
        }
        _repositoryName = repositoryName;
    }

    internal async Task<bool> DownloadRepositoryAsync()
    {
        try
        {
            string pathZip = $"{Path.Combine(_repositoryPath, _repositoryName)}.zip";
            if (File.Exists(pathZip))
            {
                File.Delete(pathZip);
            }
            using HttpClient httpClient = new();
            using HttpResponseMessage response = await httpClient.GetAsync(_urlRepository, HttpCompletionOption.ResponseContentRead);
            if (response.IsSuccessStatusCode)
            {
                using Stream contentStream = await response.Content.ReadAsStreamAsync();
                using FileStream fileStream = new FileStream(pathZip, FileMode.Create);
                await contentStream.CopyToAsync(fileStream);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        catch
        {
            throw;
        }
    }

    internal string UnzipRepository(string repositoryName)
    {
        try
        {
            string pathZip = $"{Path.Combine(_repositoryPath, _repositoryName)}.zip";
            string[] repositoryPath = Directory.GetDirectories(_repositoryPath);
            if (repositoryPath.Length > 0)
            {
                foreach (var item in repositoryPath)
                {
                    Directory.Delete(item, true);
                }
            }
            System.IO.Compression.ZipFile.ExtractToDirectory(pathZip, _repositoryPath);
            File.Delete(pathZip);
            repositoryPath = Directory.GetDirectories(_repositoryPath);
            return repositoryPath[0];
        }
        catch
        {
            throw;
        }
    }

    internal string GetDataFile(string pathFolder, string fileName)
    {
        try
        {
            var fileDir = Directory.GetFiles(pathFolder);
            fileDir = fileDir.Select(x => x.Replace($"{pathFolder}/", "")).Select(x=> x.Replace($"{pathFolder}\\", "")).ToArray();
            if (!fileDir.Contains(fileName))
            {
                return string.Empty;
            }
            string pathFile = Path.Combine(pathFolder, fileName);
            string dataFile = File.ReadAllText(pathFile);
            return dataFile;
        }
        catch
        {
            throw;
        }

    }

    internal IEnumerable<T> GetLinkConfig<T>(string dataFile, T tConfig) where T : BaseConfig, new()
    {
        try
        {   
            return tConfig.GetConfigRege<T>(dataFile).Reverse();
        }
        catch
        {
            throw;
        }
    }
}
