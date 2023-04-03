namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SenderMustNotBeAdminAttribute : UpdateHandlerAttribute
{
}