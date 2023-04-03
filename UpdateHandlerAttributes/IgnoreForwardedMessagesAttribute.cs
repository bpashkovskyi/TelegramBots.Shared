namespace TelegramBots.Shared.UpdateHandlerAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class IgnoreForwardedMessagesAttribute : UpdateHandlerAttribute
{
    public IgnoreForwardedMessagesAttribute()
    {
    }
}