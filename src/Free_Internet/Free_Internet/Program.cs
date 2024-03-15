using Free_Internet.Models;
using Free_Internet.Services;

string repositoryUrl = "https://github.com/barry-far/V2ray-Configs/archive/refs/heads/main.zip";
string repositoryName = "barry-far";
string fileName = "All_Configs_Sub.txt";

ConfigManager configManager = new(repositoryUrl, repositoryName);
bool IsDownload = await configManager.DownloadRepositoryAsync();
if (IsDownload)
{
    string pathRepoDir = configManager.UnzipRepository(repositoryName);
    string dataFile = configManager.GetDataFile(pathRepoDir,fileName);
    IEnumerable<Vless> vlessesLink = configManager.GetLinkConfig(dataFile, new Vless());
    IEnumerable<Vmess> vmessLink = configManager.GetLinkConfig(dataFile, new Vmess());
    IEnumerable<Warp> warpLink = configManager.GetLinkConfig(dataFile, new Warp());
    IEnumerable<Tuic> tuicLink = configManager.GetLinkConfig(dataFile, new Tuic());
    IEnumerable<Trojan> trojanLink = configManager.GetLinkConfig(dataFile, new Trojan());
    IEnumerable<Ss> ssLink = configManager.GetLinkConfig(dataFile, new Ss());
    IEnumerable<ShadowSocks> shadowLink = configManager.GetLinkConfig(dataFile, new ShadowSocks());
}

Console.ReadKey();