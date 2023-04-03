namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class IgnoreUsersAttribute : UpdateHandlerAttribute
{
    public IgnoreUsersAttribute(params long[] userIds)
    {
        this.UserIds = userIds;
    }

    public long[] UserIds { get; }
}