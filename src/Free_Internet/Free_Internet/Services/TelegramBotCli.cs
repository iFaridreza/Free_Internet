using TL;

namespace Free_Internet.Services;

internal class TelegramBotCli
{
    private readonly Client _clientCli;
    internal event Func<UpdatesBase, Task>? OnChannelUpdate;
    internal TelegramBotCli(int apiId, string apiHash, string? sessionPath = null)
    {
        //disable wtelegram log
        Helpers.Log = (lvl, log) => { };
        _clientCli = new Client(apiID: apiId, apiHash: apiHash, sessionPathname: sessionPath);
    }

    private void UpdateChannelInvoke(UpdatesBase updatesBase) => OnChannelUpdate?.Invoke(updatesBase);

    internal void GetUpdate()
    {
        _clientCli.OnUpdate += async (UpdatesBase update) =>
        {
            var updateInfo = update.Chats.First().Value;
            if (updateInfo.IsChannel)
            {
                await Task.Run(() => UpdateChannelInvoke(update));
            }
        };
    }

    internal async Task ConnectClient() => await _clientCli.ConnectAsync();

    internal async Task DisconnectClient() => await Task.Run(() => _clientCli.Dispose());

    internal async Task<bool> LoginUserIfNeed(string? sessionPath = null)
    {
        try
        {
            await ConnectClient();
            var usernameInfo = await _clientCli.Contacts_ResolveUsername("iFaridreza");
            return usernameInfo is null ? await Task.FromResult(false) : await Task.FromResult(true);
        }
        catch
        {
           return await Task.FromResult(false);
        }
    }

    //todo: this is unused
    internal void HttpProxy(string host, int port)
    {
        _clientCli.TcpHandler += (address, port) =>
        {
            HttpProxyClient proxyHttp = new();
            proxyHttp.ProxyHost = host;
            proxyHttp.ProxyPort = port;
            return Task.FromResult(proxyHttp.CreateConnection(address, port));
        };
    }

    internal async Task<string?> TryLoginAsync(string state)
    {
        state = await _clientCli.Login(state);
        if (_clientCli.User is not null)
        {
            state = "login_sucsess";
        }
        
        return await Task.FromResult(state);
    }
}