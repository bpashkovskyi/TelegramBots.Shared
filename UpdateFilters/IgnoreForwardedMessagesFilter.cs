namespace TelegramBots.Shared.UpdateFilters;

public class IgnoreForwardedMessagesFilter : UpdateHandlerFilter<IgnoreForwardedMessagesAttribute>
{
    public override bool Matches(IgnoreForwardedMessagesAttribute updateHandlerAttribute, Update update)
    {
        return update.Type == Telegram.Bot.Types.Enums.UpdateType.Message
            && update.Message?.ForwardFromMessageId == null;
    }
}