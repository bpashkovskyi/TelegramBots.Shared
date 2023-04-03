namespace TelegramBots.Shared.UpdateFilters;

public sealed class MessageTypesFilter : UpdateHandlerFilter<AllowedMessageTypesAttribute>
{
    public override bool Matches(AllowedMessageTypesAttribute updateHandlerAttribute, Update update)
    {
        return update.Type == Telegram.Bot.Types.Enums.UpdateType.Message
            && update.Message?.Type != null
            && updateHandlerAttribute.MessageTypes.Contains(update.Message.Type);
    }
}