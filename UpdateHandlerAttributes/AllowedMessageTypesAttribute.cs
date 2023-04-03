namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

using Telegram.Bot.Types.Enums;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class AllowedMessageTypesAttribute : UpdateHandlerAttribute
{
    public AllowedMessageTypesAttribute(params MessageType[] messageTypes)
    {
        MessageTypes = messageTypes;
    }

    public MessageType[] MessageTypes { get; }
}