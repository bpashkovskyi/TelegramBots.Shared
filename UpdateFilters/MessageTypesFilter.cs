namespace TelegramBots.Shared.UpdateFilters;

using System.Linq;

using Telegram.Bot.Types;

using TelegramBots.Shared.UpdateHandlerAttributes;

public sealed class MessageTypesFilter : UpdateHandlerFilter<AllowedMessageTypesAttribute>
{
    public override bool Matches(AllowedMessageTypesAttribute updateHandlerAttribute, Update update)
    {
        return update.Type == Telegram.Bot.Types.Enums.UpdateType.Message
            && update.Message?.Type != null
            && updateHandlerAttribute.MessageTypes.Contains(update.Message.Type);
    }
}