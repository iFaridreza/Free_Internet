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
            if(File.Exists(pathZip))
            {
                File.Delete(pathZip);
            }
            using HttpClient httpClient = new();
            using HttpResponseMessage response = await httpClient.GetAsync(_urlRepository);
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
                    Directory.Delete(item,true);
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
 
}
