try
{
    TelegramBotCli telegramBotCli = new(apiId: ConfigProject.ApiId, apiHash: ConfigProject.ApiHash);
    ConfigRepositoryManager configManager = new(ConfigProject.RepositoryUrl,ConfigProject.RepositoryName);
    TelegramBotApi telegramBot = new(ConfigProject.Token, ConfigProject.TextInlineButton, ConfigProject.UrlInlineButton);

    int GetTotalMilliseconds(int minutes) => (int)TimeSpan.FromMinutes(minutes).TotalMilliseconds;
    
    var isLogin = telegramBotCli.LoginUserIfNeed().Result;

    if (ConfigProject.PhoneNumber is not null)
    {
        string phoneNumber = ConfigProject.PhoneNumber.Replace(" ", "");
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
                if (isLogin)
                {
                    Console.WriteLine($"Login Sucsess");
                    telegramBotCli.OnChannelUpdate += TelegramBotCli_OnChannelUpdate;
                    Console.WriteLine("Ready Recive Update");
                    telegramBotCli.GetUpdate();
                    isLogin = false;
                }
            }
        }
    }

    if (isLogin)
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
        while (true)
        {
            bool IsDownload = await configManager.DownloadRepositoryAsync();
            if (IsDownload is false)
            {
                continue;
            }

            string pathRepoDir = configManager.UnzipRepository(ConfigProject.RepositoryName);
            string dataFile = configManager.GetDataFile(pathRepoDir, ConfigProject.FileName);

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
                await telegramBot.SendMessage(ConfigProject.UsernameChanellConfig, message.ToString());
                message.Clear();
                await Task.Delay(GetTotalMilliseconds(2));
            }
        }
    }

    async Task TelegramBotCli_OnChannelUpdate(TL.UpdatesBase arg)
    {
        await Task.Delay(GetTotalMilliseconds(3));

        var updateChannel = arg.Chats[0];
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
                                    await telegramBot.SendMessage(ConfigProject.UsernameChanellConfig, message.ToString());
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
                                await telegramBot.SendMessage(ConfigProject.UsernameChanellConfig, message.ToString());
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
                    await telegramBot.SendMessage(ConfigProject.UsernameChanellConfig, message.ToString());
                    message.Clear();
                }
            }
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