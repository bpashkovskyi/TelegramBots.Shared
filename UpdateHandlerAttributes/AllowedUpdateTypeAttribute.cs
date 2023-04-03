namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

using Telegram.Bot.Types.Enums;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AllowedUpdateTypeAttribute : UpdateHandlerAttribute
{
    public AllowedUpdateTypeAttribute(UpdateType updateType, string? pattern = null)
    {
        this.UpdateType = updateType;
        this.Pattern = pattern;
    }

    public UpdateType UpdateType { get; }

    public string? Pattern { get; }
}