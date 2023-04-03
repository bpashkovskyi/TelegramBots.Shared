namespace TelegramBots.Shared.UpdateFilters;

public class IgnoreMessageTypesFilter : UpdateHandlerFilter<IgnoreMessageTypesAttribute>
{
    public override bool Matches(IgnoreMessageTypesAttribute updateHandlerAttribute, Update update)
    {
        return update is { Type: UpdateType.Message, Message.Type: { } } 
               && !updateHandlerAttribute.MessageTypes.Contains(update.Message.Type);
    }
}