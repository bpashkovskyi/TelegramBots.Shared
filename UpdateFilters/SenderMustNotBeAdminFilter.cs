namespace TelegramBots.Shared.UpdateFilters;

public class SenderMustNotBeAdminFilter : UpdateHandlerFilter<SenderMustNotBeAdminAttribute>
{
    private readonly ITelegramBotClient _telegramBotClient;

    public SenderMustNotBeAdminFilter(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public override async Task<bool> MatchesAsync(SenderMustNotBeAdminAttribute updateHandlerAttribute, Update update)
    {
        var message = update.Message ?? update.EditedMessage;
        if (message == null)
        {
            return false;
        }

        var memberInfo = await _telegramBotClient.GetChatMemberAsync(message.Chat.Id, message.From!.Id).ConfigureAwait(false);
        if (memberInfo.Status != ChatMemberStatus.Administrator && memberInfo.Status != ChatMemberStatus.Creator && message.From.Username != "GroupAnonymousBot" && message.From.Username != "Telegram")
        {
            return true;
        }

        return false;
    }
}