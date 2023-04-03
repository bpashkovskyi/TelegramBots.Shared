namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public abstract class UpdateHandlerAttribute : Attribute
{
}