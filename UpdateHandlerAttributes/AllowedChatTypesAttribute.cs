namespace TelegramBots.Shared.UpdateHandlerAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AllowedChatTypesAttribute : UpdateHandlerAttribute
{
    public AllowedChatTypesAttribute(params ChatType[] chatTypes)
    {
        ChatTypes = chatTypes;
    }

    public ChatType[] ChatTypes { get; }
}