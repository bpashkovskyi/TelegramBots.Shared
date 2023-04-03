namespace TelegramBots.Shared.UpdateHandlerAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class IgnoreUsersAttribute : UpdateHandlerAttribute
{
    public IgnoreUsersAttribute(params long[] userIds)
    {
        UserIds = userIds;
    }

    public long[] UserIds { get; }
}