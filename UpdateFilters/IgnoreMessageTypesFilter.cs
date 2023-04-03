namespace TelegramBots.Shared.UpdateFilters;

using System.Linq;

using Telegram.Bot.Types;

using TelegramBots.Shared.UpdateHandlerAttributes;

public class IgnoreMessageTypesFilter : UpdateHandlerFilter<IgnoreMessageTypesAttribute>
{
    public override bool Matches(IgnoreMessageTypesAttribute updateHandlerAttribute, Update update)
    {
        return update.Type == Telegram.Bot.Types.Enums.UpdateType.Message
            && update.Message?.Type != null
            && !updateHandlerAttribute.MessageTypes.Contains(update.Message.Type);
    }
}