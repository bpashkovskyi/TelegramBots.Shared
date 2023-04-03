namespace TelegramBots.Shared.UpdateHandlerAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class IgnoreMessageTypesAttribute : UpdateHandlerAttribute
{
    public IgnoreMessageTypesAttribute(params MessageType[] messageTypes)
    {
        MessageTypes = messageTypes;
    }

    public MessageType[] MessageTypes { get; }
}