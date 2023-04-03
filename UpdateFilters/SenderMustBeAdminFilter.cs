namespace TelegramBots.Shared.UpdateFilters;

using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using TelegramBots.Shared.UpdateHandlerAttributes;

public class SenderMustBeAdminFilter : UpdateHandlerFilter<SenderMustBeAdminAttribute>
{
    private readonly ITelegramBotClient telegramBotClient;

    public SenderMustBeAdminFilter(ITelegramBotClient telegramBotClient)
    {
        this.telegramBotClient = telegramBotClient;
    }

    public override async Task<bool> MatchesAsync(SenderMustBeAdminAttribute updateHandlerAttribute, Update update)
    {
        var message = update.Message ?? update.EditedMessage;
        if (message == null)
        {
            return false;
        }

        var memberInfo = await this.telegramBotClient.GetChatMemberAsync(message.Chat.Id, message.From!.Id).ConfigureAwait(false);
        return memberInfo.Status is ChatMemberStatus.Administrator or ChatMemberStatus.Creator || message.From.Username is "GroupAnonymousBot" or "Telegram";
    }
}