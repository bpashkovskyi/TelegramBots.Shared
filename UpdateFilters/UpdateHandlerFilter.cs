namespace TelegramBots.Shared.UpdateFilters;

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
            UpdateType.Message => update.Message?.Chat,
            UpdateType.CallbackQuery => update.CallbackQuery?.Message?.Chat,
            UpdateType.EditedMessage => update.EditedMessage?.Chat,
            UpdateType.ChannelPost => update.ChannelPost?.Chat,
            UpdateType.EditedChannelPost => update.EditedChannelPost?.Chat,
            UpdateType.MyChatMember => update.MyChatMember?.Chat,
            UpdateType.ChatMember => update.ChatMember?.Chat,
            UpdateType.ChatJoinRequest => update.ChatJoinRequest?.Chat,
            _ => null
        };
    }
}