﻿namespace TelegramBots.Shared.UpdateHandlerAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MessageShouldNotBeCommandAttribute : UpdateHandlerAttribute
{
}