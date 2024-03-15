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
    int twoHour = 8400000;

    ConfigManager configManager = new(repositoryUrl, repositoryName);

    TelegramBot telegramBot = new(token, text, url);

    User? InfoBot = await telegramBot.InfoBotAsync();
    Console.WriteLine($"Bot Username @{InfoBot.Username} Run");

    telegramBot.ScheduleTaskEvent += UpdateConfig;
    telegramBot.InvokeEvent();

    System.Timers.Timer timerSchedule = new System.Timers.Timer(twoHour);
    timerSchedule.Elapsed += TimerSchedule_Elapsed;
    timerSchedule.Start();

    void TimerSchedule_Elapsed(object? sender, ElapsedEventArgs e)
    {
        telegramBot.InvokeEvent();
    }

    async Task UpdateConfig()
    {
        bool IsDownload = await configManager.DownloadRepositoryAsync();
        if (IsDownload)
        {
            string pathRepoDir = configManager.UnzipRepository(repositoryName);
            string dataFile = configManager.GetDataFile(pathRepoDir, fileName);
            IEnumerable<Vless> vlessesLink = configManager.GetLinkConfig(dataFile, new Vless());
            IEnumerable<Vmess> vmessLink = configManager.GetLinkConfig(dataFile, new Vmess());
            IEnumerable<Warp> warpLink = configManager.GetLinkConfig(dataFile, new Warp());
            IEnumerable<Tuic> tuicLink = configManager.GetLinkConfig(dataFile, new Tuic());
            IEnumerable<Trojan> trojanLink = configManager.GetLinkConfig(dataFile, new Trojan());
            IEnumerable<Ss> ssLink = configManager.GetLinkConfig(dataFile, new Ss());
            IEnumerable<ShadowSocks> shadowLink = configManager.GetLinkConfig(dataFile, new ShadowSocks());

            StringBuilder message = new();
            foreach (var vle in vlessesLink)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {nameof(Vless)} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(5000);
            }

            foreach (var vle in vmessLink)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {nameof(Vmess)} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(5000);
            }

            foreach (var vle in warpLink)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {nameof(Warp)} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(5000);
            }

            foreach (var vle in tuicLink)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {nameof(Tuic)} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(5000);
            }

            foreach (var vle in trojanLink)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {nameof(Trojan)} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(5000);
            }

            foreach (var vle in ssLink)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {nameof(Ss)} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(5000);
            }

            foreach (var vle in shadowLink)
            {
                message.Append("❤️ New Config");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"✨ Type <b>[ {nameof(ShadowSocks)} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(5000);
            }
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