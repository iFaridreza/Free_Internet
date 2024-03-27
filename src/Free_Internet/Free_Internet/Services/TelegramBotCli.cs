using TL;

namespace Free_Internet.Services;

internal class TelegramBotCli
{
    private readonly Client _clientCli;
    internal event Func<UpdatesBase, Task>? OnChannelUpdate;
    internal readonly string _usernameChannel;
    internal TelegramBotCli(int apiId, string apiHash, string usernameChannel, string? sessionPath = null)
    {
        //disable wtelegram log
        Helpers.Log = (lvl, log) => { };
        _usernameChannel = usernameChannel.Replace("@", "");
        _clientCli = new(apiID: apiId, apiHash: apiHash, sessionPathname: sessionPath);
    }

    private async Task UpdateChennelInvoke(UpdatesBase updatesBase)
    {
        await Task.Run(() =>
        {
            OnChannelUpdate?.Invoke(updatesBase);
        });
    }

    internal void GetUpdateChannel()
    {
        _clientCli.OnUpdate += async (UpdatesBase update) =>
        {
            var updateInfo = update.Chats.First().Value;
            if (updateInfo.IsChannel is true)
            {
                await UpdateChennelInvoke(update);
            }
        };
    }

    internal async Task ConnectClient()
    {
        await _clientCli.ConnectAsync();
    }

    internal async Task DisconnectClient()
    {
        await Task.Run(() => _clientCli.Dispose());
    }

    internal async Task<bool> LoginUserIfNeed(string? sessionPath = null)
    {
        try
        {
            await ConnectClient();
            var usernameInfo = await _clientCli.Contacts_ResolveUsername(_usernameChannel);
            return usernameInfo is null ? await Task.FromResult(false) : await Task.FromResult(true);
        }
        catch
        {
            return await Task.FromResult(false);
        }
    }

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