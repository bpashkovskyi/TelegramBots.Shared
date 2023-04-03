namespace TelegramBots.Shared;

using System.Collections.Generic;
using System.Threading.Tasks;

using Rollbar;

using Telegram.Bot;
using Telegram.Bot.Types;

public abstract class UpdateHandler
{
    protected UpdateHandler(IRollbar rollbar, ITelegramBotClient telegramBotClient)
    {
        Rollbar = rollbar;
        TelegramBotClient = telegramBotClient;
    }

    public Dictionary<string, string> Arguments { get; } = new();

    public virtual int Order => 0;

    protected IRollbar Rollbar { get; }

    protected ITelegramBotClient TelegramBotClient { get; }

    public abstract Task HandleAsync(Update update);
}