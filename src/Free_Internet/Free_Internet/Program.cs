//todo: use serilog
//todo: add configuration file
//todo: add command line parser

LoggerManager logger = new();
StringBuilder message = new();

if (args.Length < 2)
{
    logger.LogError("Token or Username is Null => Free_Internet.exe \"{token}\" \"{channelUsername}\"",
        "Token", "Channel Username");
    return;
}

try
{
    string token = args[0];
    string channelUsername = args[1];
    
    if (channelUsername.Contains("https://t.me/") || channelUsername.Contains("t.me/"))
    {
        channelUsername = channelUsername.Replace("https://t.me/", "@").Replace("t.me/", "@");
    }
    if (!channelUsername.Contains('@'))
    {
        channelUsername = channelUsername.Insert(0, "@");
    }

    //todo: move them to resources or config
    const string repositoryUrl = "https://github.com/barry-far/V2ray-Configs/archive/refs/heads/main.zip";
    const string repositoryName = "barry-far";
    const string fileName = "All_Configs_Sub.txt";
    const string text = "Internet is free for everyone 🌍";
    const string url = "https://t.me/iFaridreza";
    const string apiHash = "bbef22a24eda21653632cc4e1a129742";
    const int apiId = 2836318;

    int MinutesToMilliseconds(int minutes) => (int)TimeSpan.FromMinutes(minutes).TotalMilliseconds;

    TelegramBotCli telegramBotCli = new(apiId: apiId, apiHash: apiHash);
    ConfigManager configManager = new(repositoryUrl, repositoryName);
    TelegramBot telegramBot = new(token, text, url);

    //todo: see what is this
    var isLogin = telegramBotCli.LoginUserIfNeed().Result;

    //todo: refactor this to use system.command line
    if (args.Length > 2)
    {
        string phoneNumber = args[2].Replace(" ", "");
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
                            if (string.IsNullOrEmpty(code))
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
                            if (string.IsNullOrEmpty(password))
                            {
                                break;
                            }
                            stateLogin = await telegramBotCli.TryLoginAsync(password);
                        }
                        break;
                }
            }
            // todo: potential bug typo for success
            if (stateLogin == "login_sucsess")
            {
                isLogin = telegramBotCli.LoginUserIfNeed().Result;
                if (isLogin)
                {
                    logger.LogInfo("Login Success");
                    //todo: HeapView.ObjectAllocation.Possible
                    telegramBotCli.OnChannelUpdate += TelegramBotCliOnChannelUpdate;
                    logger.LogInfo("Ready to Receive Update");
                    telegramBotCli.GetUpdate();
                }
            }
        }
    }

    if (isLogin)
    {
        telegramBotCli.OnChannelUpdate += TelegramBotCliOnChannelUpdate;
        logger.LogInfo("Ready to Receive Update");
        telegramBotCli.GetUpdate();
    }


    User? infoBot = await telegramBot.InfoBotAsync();
    logger.LogInfo("Bot Username {username} Run", infoBot.Username!);

    telegramBot.ScheduleTaskEvent += UpdateConfig;

    telegramBot.InvokeEvent();

    //todo: add retry option
    async Task UpdateConfig()
    {
        while (true)
        {
            bool isDownload = await configManager.DownloadRepositoryAsync();
            if (isDownload is false)
            {
                break;
            }

            string pathRepoDir = configManager.UnzipRepository(repositoryName);
            string dataFile = ConfigManager.GetDataFile(pathRepoDir, fileName);
            
            List<IConfig> baseConfigs = new();

            baseConfigs.AddRange(configManager.GetLinkConfig(dataFile, new Vless()));
            baseConfigs.AddRange(configManager.GetLinkConfig(dataFile, new Vmess()));
            baseConfigs.AddRange(configManager.GetLinkConfig(dataFile, new Warp()));
            baseConfigs.AddRange(configManager.GetLinkConfig(dataFile, new Tuic()));
            baseConfigs.AddRange(configManager.GetLinkConfig(dataFile, new Trojan()));
            baseConfigs.AddRange(configManager.GetLinkConfig(dataFile, new Ss()));
            baseConfigs.AddRange(configManager.GetLinkConfig(dataFile, new ShadowSocks()));
            
            baseConfigs = baseConfigs.OrderBy(x => Guid.NewGuid()).ToList();

            message.Clear();
            //todo: maybe a more suitable name?
            foreach (var vle in baseConfigs)
            {
                message.Append("❤️ New Config");
                message.AppendLine();
                message.AppendLine();
                message.Append($"✨ Type <b>[ #{vle.ConfigType} ]</b>");
                message.AppendLine();
                message.AppendLine();
                message.Append($"<code>{vle.Link}</code>");
                message.AppendLine();
                message.AppendLine();
                message.Append($"#Free_Internet ");
                await telegramBot.SendMessage(channelUsername, message.ToString());
                message.Clear();
                
                //todo: its really easier to just use 2 * 60 * 1000 ? isn't it?
                await Task.Delay(MinutesToMilliseconds(2));
            }
        }
    }

    //todo: any better name than arg like update?
    async Task TelegramBotCliOnChannelUpdate(TL.UpdatesBase arg)
    {
        await Task.Delay(MinutesToMilliseconds(3));

        //todo: any way to extract this?
        var updateChannel = arg.Chats.First().Value;
        if (updateChannel is null) return;
        message.Clear();            
        foreach (var item in arg.UpdateList)
        {
            TL.UpdateNewChannelMessage updateChanell = (TL.UpdateNewChannelMessage)item;
            TL.Message messageUpdate = (TL.Message)updateChanell.message;
            TL.MessageEntity[] messageEntity = (TL.MessageEntity[])messageUpdate.entities;
            TL.ReplyInlineMarkup messageMarkup = (TL.ReplyInlineMarkup)messageUpdate.reply_markup;

            string? resultProxy;

            //todo: move this to func
            if (messageMarkup is not null)
            {
                foreach (var inline in messageMarkup.rows)
                {
                    foreach (var button in inline.buttons)
                    {
                        TL.KeyboardButtonUrl keyboardButton = (TL.KeyboardButtonUrl)button;
                            
                        if (keyboardButton is null) continue;
                        string url = keyboardButton.url;
                        resultProxy = IsProxy(url);

                        if (string.IsNullOrEmpty(resultProxy)) continue;
                            
                        message.Append("❤️ New Proxy");
                        message.AppendLine();
                        message.AppendLine();
                        message.Append($"✨ Type <b>[ #Proxy ]</b>");
                        message.AppendLine();
                        message.AppendLine();
                        message.Append($"{resultProxy}");
                        message.AppendLine();
                        message.AppendLine();
                        message.Append($"#Free_Internet ");
                        await telegramBot.SendMessage(channelUsername, message.ToString());
                        message.Clear();
                    }
                }
            }

            //todo: why code duplication? why not use a function?
            if (messageEntity is not null)
            {
                foreach (var entity in messageEntity)
                {
                    if (entity is not TL.MessageEntityTextUrl entityUrl) continue;
                        
                    var url = entityUrl.url;
                    resultProxy = IsProxy(url);

                    if (string.IsNullOrEmpty(resultProxy)) continue;
                        
                    message.Append("❤️ New Proxy");
                    message.AppendLine();
                    message.AppendLine();
                    message.Append($"✨ Type <b>[ #Proxy ]</b>");
                    message.AppendLine();
                    message.AppendLine();
                    message.Append($"{resultProxy}");
                    message.AppendLine();
                    message.AppendLine();
                    message.Append($"#Free_Internet ");
                    await telegramBot.SendMessage(channelUsername, message.ToString());
                    message.Clear();
                }
            }
            string messageText = messageUpdate.message;
            resultProxy = IsProxy(messageText);

            //todo: oh god another one
            if (string.IsNullOrEmpty(resultProxy)) continue;
                
            message.Append("❤️ New Proxy");
            message.AppendLine();
            message.AppendLine();
            message.Append($"✨ Type <b>[ #Proxy ]</b>");
            message.AppendLine();
            message.AppendLine();
            message.Append($"{resultProxy}");
            message.AppendLine();
            message.AppendLine();
            message.Append($"#Free_Internet ");
            await telegramBot.SendMessage(channelUsername, message.ToString());
            message.Clear();
        }

        return;

        string? IsProxy(string text)
        {
            var matches = ProxyRegex().Match(text);
            return matches.Value;
        }
    }

    //todo: maybe use service?
    Console.ReadKey();
    //todo: and proper exiting
    await telegramBotCli.DisconnectClient();
}
catch (Exception ex)
{
    logger.LogFatal(ex, ex.Message);
}

internal partial class Program
{
    [GeneratedRegex(@"https:\/\/t.me\/proxy\?(.*)")]
    private static partial Regex ProxyRegex();
}
