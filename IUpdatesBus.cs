namespace TelegramBots.Shared;

public interface IUpdatesBus
{
    Task SendAsync(Update update);
}