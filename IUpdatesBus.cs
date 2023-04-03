namespace TelegramBots.Shared;

using System.Threading.Tasks;

using Telegram.Bot.Types;

public interface IUpdatesBus
{
    Task SendAsync(Update update);
}