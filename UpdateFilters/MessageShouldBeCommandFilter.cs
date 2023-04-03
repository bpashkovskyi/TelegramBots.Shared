namespace TelegramBots.Shared.UpdateFilters;

public class MessageShouldBeCommandFilter : UpdateHandlerFilter<MessageShouldBeCommandAttribute>
{
    public override bool Matches(MessageShouldBeCommandAttribute updateHandlerAttribute, Update update)
    {
        if (updateHandlerAttribute.CommandName == null)
        {
            return update.Type == UpdateType.Message && update.Message?.IsCommand() == true;
        }

        return update.Type == UpdateType.Message && update.Message?.IsCommand(updateHandlerAttribute.CommandName) == true;
    }
}