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
    string apiHash = "bbef22a24eda21653632cc4e1a129742";
    int apiId = 2836318;

    int threeDelayOfMilisecound = 180000;
    int oneDelayOfMilisecound = 60000;
    TelegramBotCli telegramBotCli = new(apiId: apiId, apiHash: apiHash);
    ConfigManager configManager = new(repositoryUrl, repositoryName);
    TelegramBot telegramBot = new(token, text, url);

    var isLogin = telegramBotCli.LoginUserIfNeed().Result;

    if (args.Length > 2)
    {
        string phoneNumber = args[2].Replace(" ","");
        if (isLogin is false)
        {
            string? stateLogin = await telegramBotCli.TryLoginAsync(phoneNumber);
            while (stateLogin != "login_sucsess")
            {
                switch (stateLogin)
                {
                    case "verification_code":
                        {
                            Console.Write("Code: ");
                            string? code = Console.ReadLine();
                            if (String.IsNullOrEmpty(code))
                            {
                                break;
                            }
                            stateLogin = await telegramBotCli.TryLoginAsync(code);
                        }
                        break;
                    case "password":
                        {
                            Console.Write("Password: ");
                            string? password = Console.ReadLine();
                            if (String.IsNullOrEmpty(password))
                            {
                                break;
                            }
                            stateLogin = await telegramBotCli.TryLoginAsync(password);
                        }
                        break;
                }
            }
            if (stateLogin == "login_sucsess")
            {
                isLogin = telegramBotCli.LoginUserIfNeed().Result;
                if (isLogin is true)
                {
                    Console.WriteLine($"Login Sucsess");
                    telegramBotCli.OnChannelUpdate += TelegramBotCli_OnChannelUpdate;
                    Console.WriteLine("Ready Recive Update");
                    telegramBotCli.GetUpdate();
                }
            }
        }
    }

    if (isLogin is true)
    {
        telegramBotCli.OnChannelUpdate += TelegramBotCli_OnChannelUpdate;
        Console.WriteLine("Ready Recive Update");
        telegramBotCli.GetUpdate();
    }


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
                message.Append($"✨ Type <b>[ #{vle.ConfigType} ]</b>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"<code>{vle.Link}</code>");
                message.Append(Environment.NewLine);
                message.Append(Environment.NewLine);
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(usernameChanell, message.ToString());
                message.Clear();
                await Task.Delay(threeDelayOfMilisecound);
            }

            telegramBot.InvokeEvent();
        }
    }

    async Task TelegramBotCli_OnChannelUpdate(TL.UpdatesBase arg)
    {
        var updateChannel = arg.Chats.First().Value;
        if (updateChannel is not null)
        {
            StringBuilder message = new();
            foreach (var item in arg.UpdateList)
            {
                TL.UpdateNewChannelMessage updateChanell = (TL.UpdateNewChannelMessage)item;
                TL.Message messageUpdate = (TL.Message)updateChanell.message;
                TL.MessageEntity[] messageEntity = (TL.MessageEntity[])messageUpdate.entities;
                TL.ReplyInlineMarkup messageMarkup = (TL.ReplyInlineMarkup)messageUpdate.reply_markup;

                string? resultProxy = null;

                if (messageMarkup is not null)
                {
                    foreach (var inline in messageMarkup.rows)
                    {
                        foreach (var button in inline.buttons)
                        {
                            TL.KeyboardButtonUrl keyboardButton = (TL.KeyboardButtonUrl)button;
                            if (keyboardButton is not null)
                            {
                                string url = keyboardButton.url;
                                resultProxy = IsProxy(url);

                                if (!String.IsNullOrEmpty(resultProxy))
                                {
                                    message.Append("❤️ New Proxy");
                                    message.Append(Environment.NewLine);
                                    message.Append(Environment.NewLine);
                                    message.Append($"✨ Type <b>[ #Proxy ]</b>");
                                    message.Append(Environment.NewLine);
                                    message.Append(Environment.NewLine);
                                    message.Append($"{resultProxy}");
                                    message.Append(Environment.NewLine);
                                    message.Append(Environment.NewLine);
                                    message.Append($"#Free_Internet ");
                                    await telegramBot.SendMessage(usernameChanell, message.ToString());
                                    message.Clear();
                                }
                            }
                        }
                    }
                }

                if (messageEntity is not null)
                {
                    foreach (var entity in messageEntity)
                    {
                        if (entity is TL.MessageEntityTextUrl entityUrl)
                        {
                            string url = entityUrl.url;
                            resultProxy = IsProxy(url);

                            if (!String.IsNullOrEmpty(resultProxy))
                            {
                                message.Append("❤️ New Proxy");
                                message.Append(Environment.NewLine);
                                message.Append(Environment.NewLine);
                                message.Append($"✨ Type <b>[ #Proxy ]</b>");
                                message.Append(Environment.NewLine);
                                message.Append(Environment.NewLine);
                                message.Append($"{resultProxy}");
                                message.Append(Environment.NewLine);
                                message.Append(Environment.NewLine);
                                message.Append($"#Free_Internet ");
                                await telegramBot.SendMessage(usernameChanell, message.ToString());
                                message.Clear();
                            }
                        }
                    }
                }
                string messageText = messageUpdate.message;
                resultProxy = IsProxy(messageText);

                if (!String.IsNullOrEmpty(resultProxy))
                {
                    message.Append("❤️ New Proxy");
                    message.Append(Environment.NewLine);
                    message.Append(Environment.NewLine);
                    message.Append($"✨ Type <b>[ #Proxy ]</b>");
                    message.Append(Environment.NewLine);
                    message.Append(Environment.NewLine);
                    message.Append($"{resultProxy}");
                    message.Append(Environment.NewLine);
                    message.Append(Environment.NewLine);
                    message.Append($"#Free_Internet ");
                    await telegramBot.SendMessage(usernameChanell, message.ToString());
                    message.Clear();
                }
            }
            await Task.Delay(oneDelayOfMilisecound);
        }

        string? IsProxy(string text)
        {
            const string pattern = @"https:\/\/t.me\/proxy\?(.*)";

            var matchs = Regex.Match(text, pattern);
            return matchs.Value ?? null;
        }
    }

    Console.ReadKey();
    await telegramBotCli.DisconnectClient();
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