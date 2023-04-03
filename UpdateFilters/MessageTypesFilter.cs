namespace TelegramBots.Shared.UpdateFilters;

public sealed class MessageTypesFilter : UpdateHandlerFilter<AllowedMessageTypesAttribute>
{
    public override bool Matches(AllowedMessageTypesAttribute updateHandlerAttribute, Update update)
    {
        return update is { Type: UpdateType.Message, Message.Type: { } } 
               && updateHandlerAttribute.MessageTypes.Contains(update.Message.Type);
    }
}