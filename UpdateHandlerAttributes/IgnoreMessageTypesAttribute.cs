namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

using Telegram.Bot.Types.Enums;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class IgnoreMessageTypesAttribute : UpdateHandlerAttribute
{
    public IgnoreMessageTypesAttribute(params MessageType[] messageTypes)
    {
        this.MessageTypes = messageTypes;
    }

    public MessageType[] MessageTypes { get; }
}