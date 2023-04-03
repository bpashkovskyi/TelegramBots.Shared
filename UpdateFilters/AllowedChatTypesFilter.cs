namespace TelegramBots.Shared.UpdateFilters;

public class AllowedChatTypesFilter : UpdateHandlerFilter<AllowedChatTypesAttribute>
{
    public override bool Matches(AllowedChatTypesAttribute updateHandlerAttribute, Update update)
    {
        var chat = this.GetChat(update);
        return chat != null && updateHandlerAttribute.ChatTypes.Contains(chat.Type);
    }
}