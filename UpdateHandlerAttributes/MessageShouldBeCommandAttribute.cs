namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MessageShouldBeCommandAttribute : UpdateHandlerAttribute
{
    public MessageShouldBeCommandAttribute(string commandName)
    {
        CommandName = commandName;
    }

    public MessageShouldBeCommandAttribute()
    {
    }

    public string? CommandName { get; }
}