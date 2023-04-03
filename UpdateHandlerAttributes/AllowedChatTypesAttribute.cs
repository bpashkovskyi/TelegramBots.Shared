namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

using Telegram.Bot.Types.Enums;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AllowedChatTypesAttribute : UpdateHandlerAttribute
{
    public AllowedChatTypesAttribute(params ChatType[] chatTypes)
    {
        this.ChatTypes = chatTypes;
    }

    public ChatType[] ChatTypes { get; }
}