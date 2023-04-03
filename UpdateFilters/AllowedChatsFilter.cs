namespace TelegramBots.Shared.UpdateFilters;

using System.Linq;

using Telegram.Bot.Types;

using TelegramBots.Shared.UpdateHandlerAttributes;

public class AllowedChatsFilter : UpdateHandlerFilter<AllowedChatsAttribute>
{
    public override bool Matches(AllowedChatsAttribute updateHandlerAttribute, Update update)
    {
        var chat = this.GetChat(update);
        return chat != null && updateHandlerAttribute.ChatIds.Contains(chat.Id);
    }
}