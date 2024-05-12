using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace Free_Internet.Services;
internal class TelegramBotApi
{
    private readonly TelegramBotClient _botClient;
    private readonly string _text;
    private readonly string _url;

    //todo: this is unused
    internal event Func<Task>? ScheduleTaskEvent;

    internal TelegramBotApi(string token, string text, string url)
    {
        _botClient = new TelegramBotClient(token);
        _text = text;
        _url = url;
    }

    //todo: this is unused
    internal void InvokeEvent() => ScheduleTaskEvent?.Invoke();

    //todo: this is unused
    internal async Task<User> InfoBotAsync() => await _botClient.GetMeAsync();

    //todo: this is unused
    internal async Task SendMessage(string usernameChanell, string message) => 
        await _botClient.SendTextMessageAsync(usernameChanell, message, parseMode: ParseMode.Html,
            replyMarkup: DisplayButton(_text, _url));

    private static InlineKeyboardMarkup DisplayButton(string text, string url)
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
