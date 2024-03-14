using Free_Internet.Services;

string repositoryUrl = "https://github.com/barry-far/V2ray-Configs/archive/refs/heads/main.zip";
string repositoryName = "barry-far";

ConfigManager configManager = new(repositoryUrl, repositoryName);
bool IsDownload = await configManager.DownloadRepositoryAsync();
if (IsDownload)
{
    string pathRepoDir = configManager.UnzipRepository(repositoryName);
}

Console.ReadKey();