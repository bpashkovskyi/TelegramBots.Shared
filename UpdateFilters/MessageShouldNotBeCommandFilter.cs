namespace TelegramBots.Shared.UpdateFilters;

public class MessageShouldNotBeCommandFilter : UpdateHandlerFilter<MessageShouldNotBeCommandAttribute>
{
    public override bool Matches(MessageShouldNotBeCommandAttribute updateHandlerAttribute, Update update)
    {
        return update.Message?.IsCommand() == false;
    }
}