using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace Free_Internet.Services;
internal class TelegramBot
{
    private TelegramBotClient _botClient;
    private string _text, _url;

    internal event Func<Task>? ScheduleTaskEvent;

    internal TelegramBot(string token, string text, string url)
    {
        _botClient = new(token);
        _text = text;
        _url = url;
    }

    internal void InvokeEvent()
    {
        ScheduleTaskEvent?.Invoke();
    }

    internal async Task<User> InfoBotAsync()
    {
        try
        {
            return await _botClient.GetMeAsync();
        }
        catch
        {
            throw;
        }
    }

    internal async Task SendMessage(string usernameChanell, string message)
    {
        try
        {
            await _botClient.SendTextMessageAsync(usernameChanell, message, parseMode: ParseMode.Html, replyMarkup: DisplayButton(_text, _url));
        }
        catch
        {
            throw;
        }
    }

    private InlineKeyboardMarkup DisplayButton(string text, string url)
    {

        InlineKeyboardMarkup button = new(new[]
        {
            new InlineKeyboardButton[]
            {
                new InlineKeyboardButton(text)
                {
                    Url = url
                }
            }
        });
        return button;
    }

}
