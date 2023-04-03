namespace TelegramBots.Shared.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Rollbar;

using Telegram.Bot.Types;

[ApiController]
public class MessageController : Controller
{
    private readonly IRollbar rollbar;
    private readonly IUpdatesBus updatesBus;

    public MessageController(IUpdatesBus updatesBus, IRollbar rollbar)
    {
        this.updatesBus = updatesBus;
        this.rollbar = rollbar;
    }

    [HttpPost("")]
    public async Task<OkResult> HandleUpdateAsync(Update update)
    {
        try
        {
            await this.updatesBus.SendAsync(update).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            this.rollbar.Critical(
                exception,
                new Dictionary<string, object?> { { "update", update } });
        }

        return this.Ok();
    }

    [HttpGet("")]
    public string Test()
    {
        return "It works";
    }
}