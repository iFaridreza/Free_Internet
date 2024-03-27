using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace Free_Internet.Services;
internal class TelegramBotApi
{
    private TelegramBotClient _botClient;
    private string _text, _url;
    internal event Func<Task>? ScheduleTaskEvent;

    internal TelegramBotApi(string token, string text, string url)
    {
        _botClient = new(token);
        _text = text;
        _url = url;
    }

    internal async Task InvokeEvent()
    {
        await Task.Run(() => { ScheduleTaskEvent?.Invoke(); });
    }

    internal async Task<User> InfoBotAsync()
    {
        return await _botClient.GetMeAsync();
    }

    internal async Task SendMessage(string usernameChanell, string message)
    {
        await _botClient.SendTextMessageAsync(usernameChanell, message, parseMode: ParseMode.Html, replyMarkup: DisplayButton(_text, _url));
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
