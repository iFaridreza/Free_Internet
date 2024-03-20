using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace Free_Internet.Services;
internal class TelegramBot
{
    private readonly TelegramBotClient _botClient;
    private readonly string _text;
    private readonly string _url;

    internal event Func<Task>? ScheduleTaskEvent;

    internal TelegramBot(string token, string text, string url)
    {
        _botClient = new TelegramBotClient(token);
        _text = text;
        _url = url;
    }

    internal void InvokeEvent() => ScheduleTaskEvent?.Invoke();

    internal async Task<User> InfoBotAsync() => await _botClient.GetMeAsync();

    internal async Task SendMessage(string channelUsername, string message) => 
        await _botClient
            .SendTextMessageAsync(channelUsername, message, parseMode: ParseMode.Html,
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
