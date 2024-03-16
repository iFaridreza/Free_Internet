try
{
    string token = args[0];
    string usernameChanell = args[1];


    if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(usernameChanell))
    {
        Console.WriteLine("Token or Username is Null => Free_Internet.exe \"Token\" \"Username Chanell\"");
        return;
    }
    if (usernameChanell.Contains("https://t.me/") || usernameChanell.Contains("t.me/"))
    {
        usernameChanell = usernameChanell.Replace("https://t.me/", "@").Replace("t.me/", "@");
    }
    if (!usernameChanell.Contains("@"))
    {
        usernameChanell = usernameChanell.Insert(0, "@");
    }

    string repositoryUrl = "https://github.com/barry-far/V2ray-Configs/archive/refs/heads/main.zip";
    string repositoryName = "barry-far";
    string fileName = "All_Configs_Sub.txt";
    string text = "Internet is free for everyone 🌍";
    string url = "https://t.me/iFaridreza";
    int delayOfMilisecound = 60000;

    ConfigManager configManager = new(repositoryUrl, repositoryName);

    TelegramBot telegramBot = new(token, text, url);

    User? InfoBot = await telegramBot.InfoBotAsync();
    Console.WriteLine($"Bot Username @{InfoBot.Username} Run");

    telegramBot.ScheduleTaskEvent += UpdateConfig;
    telegramBot.InvokeEvent();

    async Task UpdateConfig()
    {
        bool IsDownload = await configManager.DownloadRepositoryAsync();
        if (IsDownload)
        {
            string pathRepoDir = configManager.UnzipRepository(repositoryName);
            string dataFile = configManager.GetDataFile(pathRepoDir, fileName);

            List<Vless> vlessesLink = configManager.GetLinkConfig(dataFile, new Vless()).ToList();
            List<Vmess> vmessLink = configManager.GetLinkConfig(dataFile, new Vmess()).ToList();
            List<Warp> warpLink = configManager.GetLinkConfig(dataFile, new Warp()).ToList();
            List<Tuic> tuicLink = configManager.GetLinkConfig(dataFile, new Tuic()).ToList();
            List<Trojan> trojanLink = configManager.GetLinkConfig(dataFile, new Trojan()).ToList();
            List<Ss> ssLink = configManager.GetLinkConfig(dataFile, new Ss()).ToList();
            List<ShadowSocks> shadowLink = configManager.GetLinkConfig(dataFile, new ShadowSocks()).ToList();

            List<BaseConfig> baseConfigs = new();
            baseConfigs.AddRange(vlessesLink);
            baseConfigs.AddRange(vmessLink);
            baseConfigs.AddRange(warpLink);
            baseConfigs.AddRange(tuicLink);
            baseConfigs.AddRange(trojanLink);
            baseConfigs.AddRange(ssLink);
            baseConfigs.AddRange(shadowLink);

            baseConfigs = baseConfigs.OrderBy(x => Guid.NewGuid()).ToList();

            StringBuilder message = new();

            foreach (var vle in baseConfigs)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {vle.ConfigType} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(delayOfMilisecound);
            }

            telegramBot.InvokeEvent();
        }
    }
}
catch (Exception ex)
{
    string pathErrorFile = "LogError.txt";
    if (System.IO.File.Exists(pathErrorFile))
    {
        System.IO.File.Create(pathErrorFile).Close();
    }
    string errorMessage = $"{DateTime.Now}\n\n{ex.Message}\n\n{ex.StackTrace}\n======= ++ =======\n";
    System.IO.File.AppendText(errorMessage);
}
Console.ReadKey();