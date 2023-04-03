namespace TelegramBots.Shared.UpdateFilters;

using System.Threading.Tasks;

using Telegram.Bot.Types;

using TelegramBots.Shared.UpdateHandlerAttributes;

public abstract class UpdateHandlerFilter<T>
where T : UpdateHandlerAttribute
{
    public virtual bool Matches(T updateHandlerAttribute, Update update)
    {
        return false;
    }

    public virtual Task<bool> MatchesAsync(T updateHandlerAttribute, Update update)
    {
        return Task.FromResult(false);
    }

    public virtual void SetHandlerArguments(T updateHandlerAttribute, UpdateHandler updateHandler, Update update)
    {
    }

    protected Chat? GetChat(Update update)
    {
        return update.Type switch
        {
            Telegram.Bot.Types.Enums.UpdateType.Message => update.Message?.Chat,
            Telegram.Bot.Types.Enums.UpdateType.CallbackQuery => update.CallbackQuery?.Message?.Chat,
            Telegram.Bot.Types.Enums.UpdateType.EditedMessage => update.EditedMessage?.Chat,
            Telegram.Bot.Types.Enums.UpdateType.ChannelPost => update.ChannelPost?.Chat,
            Telegram.Bot.Types.Enums.UpdateType.EditedChannelPost => update.EditedChannelPost?.Chat,
            Telegram.Bot.Types.Enums.UpdateType.MyChatMember => update.MyChatMember?.Chat,
            Telegram.Bot.Types.Enums.UpdateType.ChatMember => update.ChatMember?.Chat,
            Telegram.Bot.Types.Enums.UpdateType.ChatJoinRequest => update.ChatJoinRequest?.Chat,
            _ => null
        };
    }
}