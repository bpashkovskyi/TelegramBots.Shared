namespace TelegramBots.Shared.UpdateFilters;

public class AllowedChatsFilter : UpdateHandlerFilter<AllowedChatsAttribute>
{
    public override bool Matches(AllowedChatsAttribute updateHandlerAttribute, Update update)
    {
        var chat = GetChat(update);
        return chat != null && updateHandlerAttribute.ChatIds.Contains(chat.Id);
    }
}