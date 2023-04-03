﻿namespace TelegramBots.Shared.UpdateHandlerAttributes;

using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AllowedChatsAttribute : UpdateHandlerAttribute
{
    public AllowedChatsAttribute(params long[] chatIds)
    {
        this.ChatIds = chatIds;
    }

    public long[] ChatIds { get; }
}