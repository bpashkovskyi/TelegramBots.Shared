namespace TelegramBots.Shared.UpdateFilters;

using Telegram.Bot.Types;

using TelegramBots.Shared.Extensions;
using TelegramBots.Shared.UpdateHandlerAttributes;

public class MessageShouldBeCommandFilter : UpdateHandlerFilter<MessageShouldBeCommandAttribute>
{
    public override bool Matches(MessageShouldBeCommandAttribute updateHandlerAttribute, Update update)
    {
        if (updateHandlerAttribute.CommandName == null)
        {
            return update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message?.IsCommand() == true;
        }

        return update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message?.IsCommand(updateHandlerAttribute.CommandName) == true;
    }
}