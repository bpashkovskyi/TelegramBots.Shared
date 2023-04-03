using System.Text.RegularExpressions;

namespace TelegramBots.Shared.UpdateFilters;

public sealed class AllowedUpdateTypeFilter : UpdateHandlerFilter<AllowedUpdateTypeAttribute>
{
    public override bool Matches(AllowedUpdateTypeAttribute updateHandlerAttribute, Update update)
    {
        if (updateHandlerAttribute.Pattern != null)
        {
            var regex = new Regex(updateHandlerAttribute.Pattern, RegexOptions.Singleline);

            var data = this.GetData(update);
            return update.Type == updateHandlerAttribute.UpdateType && data != null && regex.IsMatch(data);
        }

        return update.Type == updateHandlerAttribute.UpdateType;
    }

    public override void SetHandlerArguments(AllowedUpdateTypeAttribute updateHandlerAttribute, UpdateHandler updateHandler, Update update)
    {
        var data = this.GetData(update);
        if (data == null || updateHandlerAttribute.Pattern == null)
        {
            return;
        }

        var regex = new Regex(updateHandlerAttribute.Pattern, RegexOptions.Singleline);
        var match = regex.Match(data);

        if (match.Success)
        {
            foreach (var groupName in regex.GetGroupNames())
            {
                updateHandler.Arguments.Add(groupName, match.Groups[groupName].Value);
            }
        }
    }

    private string? GetData(Update update)
    {
        return update.Type switch
        {
            Telegram.Bot.Types.Enums.UpdateType.Message => update.Message?.Text,
            Telegram.Bot.Types.Enums.UpdateType.CallbackQuery => update.CallbackQuery?.Data,
            Telegram.Bot.Types.Enums.UpdateType.ChannelPost => update.ChannelPost?.Text,
            _ => null
        };
    }
}