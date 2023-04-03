namespace TelegramBots.Shared.UpdateFilters;

using Telegram.Bot.Types;

using TelegramBots.Shared.Extensions;
using TelegramBots.Shared.UpdateHandlerAttributes;

public class MessageShouldNotBeCommandFilter : UpdateHandlerFilter<MessageShouldNotBeCommandAttribute>
{
    public override bool Matches(MessageShouldNotBeCommandAttribute updateHandlerAttribute, Update update)
    {
        return update.Message?.IsCommand() == false;
    }
}