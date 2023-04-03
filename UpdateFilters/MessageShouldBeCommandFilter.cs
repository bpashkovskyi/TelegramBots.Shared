using TelegramBots.Shared.Extensions;

namespace TelegramBots.Shared.UpdateFilters;

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