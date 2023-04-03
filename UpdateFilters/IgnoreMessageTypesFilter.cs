namespace TelegramBots.Shared.UpdateFilters;

public class IgnoreMessageTypesFilter : UpdateHandlerFilter<IgnoreMessageTypesAttribute>
{
    public override bool Matches(IgnoreMessageTypesAttribute updateHandlerAttribute, Update update)
    {
        return update.Type == Telegram.Bot.Types.Enums.UpdateType.Message
            && update.Message?.Type != null
            && !updateHandlerAttribute.MessageTypes.Contains(update.Message.Type);
    }
}