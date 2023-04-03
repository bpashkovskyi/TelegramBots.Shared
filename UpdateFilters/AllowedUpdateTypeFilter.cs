using System.Text.RegularExpressions;

namespace TelegramBots.Shared.UpdateFilters;

public sealed class AllowedUpdateTypeFilter : UpdateHandlerFilter<AllowedUpdateTypeAttribute>
{
    public override bool Matches(AllowedUpdateTypeAttribute updateHandlerAttribute, Update update)
    {
        if (updateHandlerAttribute.Pattern != null)
        {
            var regex = new Regex(updateHandlerAttribute.Pattern, RegexOptions.Singleline);

            var data = GetData(update);
            return update.Type == updateHandlerAttribute.UpdateType && data != null && regex.IsMatch(data);
        }

        return update.Type == updateHandlerAttribute.UpdateType;
    }

    public override void SetHandlerArguments(AllowedUpdateTypeAttribute updateHandlerAttribute, UpdateHandler updateHandler, Update update)
    {
        var data = GetData(update);
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
            UpdateType.Message => update.Message?.Text,
            UpdateType.CallbackQuery => update.CallbackQuery?.Data,
            UpdateType.ChannelPost => update.ChannelPost?.Text,
            _ => null
        };
    }
}