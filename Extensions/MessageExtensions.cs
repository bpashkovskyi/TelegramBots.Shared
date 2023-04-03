using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TelegramBots.Shared.Extensions;

public static class MessageExtensions
{
    public static string? GetMessageText(this Message message)
    {
        var commandName = message.GetCommandName();
        if (commandName == null)
        {
            return message.Text;
        }

        return message.Text!.Replace($"/{commandName} ", string.Empty, StringComparison.Ordinal);
    }

    public static long? TextAsLong(this Message message)
    {
        var messageText = message.GetMessageText();
        if (messageText == null)
        {
            return null;
        }

        if (long.TryParse(messageText, out var textAsLong))
        {
            return textAsLong;
        }

        return null;
    }

    public static int? TextAsInt(this Message message)
    {
        var messageText = message.GetMessageText();
        if (messageText == null)
        {
            return null;
        }

        if (int.TryParse(messageText, out var textAsLong))
        {
            return textAsLong;
        }

        return null;
    }

    public static bool IsCommand(this Message message)
    {
        var botCommand = message.GetCommandName();

        return botCommand != null;
    }

    public static bool IsCommand(this Message message, string commandName)
    {
        var botCommand = message.GetCommandName();
        if (botCommand == null)
        {
            return false;
        }

        if (botCommand.Contains('@', StringComparison.OrdinalIgnoreCase))
        {
            botCommand = botCommand[..botCommand.IndexOf('@', StringComparison.OrdinalIgnoreCase)];
        }

        return botCommand.StartsWith(commandName, StringComparison.Ordinal);
    }

    public static bool IsLink(this Message message)
    {
        return message.Entities?.Any(entity => entity.Type == MessageEntityType.Url) == true ||
            message.Entities?.Any(entity => entity.Type == MessageEntityType.TextLink) == true;
    }

    public static bool ContainsAll(this Message message, params string[] subStrings)
    {
        if (message.Type == MessageType.Text)
        {
            return subStrings.All(subString => message.Text!.Contains(subString, StringComparison.OrdinalIgnoreCase));
        }

        if(message.Type is MessageType.Photo or MessageType.Video && message.Caption != null)
        {
            return subStrings.All(subString => message.Caption!.Contains(subString, StringComparison.OrdinalIgnoreCase));
        }

        return false;
    }

    public static bool ContainsAny(this Message message, params string[] subStrings)
    {
        if (message.Type == MessageType.Text)
        {
            return subStrings.Any(subString => message.Text!.Contains(subString, StringComparison.OrdinalIgnoreCase));
        }

        if (message.Type is MessageType.Photo or MessageType.Video && message.Caption != null)
        {
            return subStrings.Any(subString => message.Caption!.Contains(subString, StringComparison.OrdinalIgnoreCase));
        }

        return false;
    }

    public static bool Contains(this Message message, string subString)
    {
        if (message.Type == MessageType.Text)
        {
            return message.Text!.Contains(subString, StringComparison.OrdinalIgnoreCase);
        }

        if (message.Type is MessageType.Photo or MessageType.Video && message.Caption != null)
        {
            return message.Caption!.Contains(subString, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    public static string TextWithSenderInfo(this Message message)
    {
        return $"{message.Text}\n{message.GetSenderInfo()}";
    }

    public static string CaptionWithSenderInfo(this Message message)
    {
        return $"{message.Caption}\n{message.GetSenderInfo()}";
    }

    public static string GetSenderInfo(this Message message)
    {
        var messageStringBuilder = new StringBuilder();
        messageStringBuilder.Append("(повідомлення надіслане від користувача:");

        if (!string.IsNullOrEmpty(message.From?.FirstName))
        {
            messageStringBuilder.Append(CultureInfo.InvariantCulture, $" {message.From?.FirstName}");
        }

        if (!string.IsNullOrEmpty(message.From?.LastName))
        {
            messageStringBuilder.Append(CultureInfo.InvariantCulture, $" {message.From?.LastName},");
        }

        if (!string.IsNullOrEmpty(message.From?.Username))
        {
            messageStringBuilder.Append(CultureInfo.InvariantCulture, $" @{message.From?.Username},");
        }

        messageStringBuilder.Append(CultureInfo.InvariantCulture, $" Id: {message.From?.Id}).");

        return messageStringBuilder.ToString();
    }

    public static long? ParseUserId(this string text)
    {
        var pattern = @"Id: (?<id>[0-9]+)\).$";
        var regex = new Regex(pattern, RegexOptions.Multiline);
        var match = regex.Match(text);

        if (match.Success && match.Groups.TryGetValue("id", out var userIdGroup))
        {
            if (userIdGroup.Success && long.TryParse(userIdGroup.Value, out var userId))
            {
                return userId;
            }
        }

        return null;
    }

    private static string? GetCommandName(this Message message)
    {
        if (message.Text == null)
        {
            return null;
        }

        var botCommandEntity = message.Entities?.FirstOrDefault(entity => entity.Type == MessageEntityType.BotCommand);
        if (botCommandEntity == null)
        {
            return null;
        }

        // to extract 'foo' instead of '/foo'
        var botCommand = message.Text.Substring(botCommandEntity.Offset + 1, botCommandEntity.Length - 1);

        return botCommand;
    }
}