namespace TelegramBots.Shared.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Rollbar;

using Telegram.Bot.Types;

[ApiController]
[Route("")]
public class MessageController : Controller
{
    private readonly IRollbar _rollbar;
    private readonly IUpdatesBus _updatesBus;

    public MessageController(
        IUpdatesBus updatesBus,
        IRollbar rollbar)
    {
        this._updatesBus = updatesBus;
        this._rollbar = rollbar;
    }

    [HttpPost]
    public async Task<OkResult> HandleUpdateAsync(Update update)
    {
        try
        {
            await this._updatesBus.SendAsync(update).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            this._rollbar.Critical(
                exception,
                new Dictionary<string, object?> { { "update", update } });
        }

        return this.Ok();
    }

    [HttpGet]
    public string Test()
    {
        return "It works";
    }
}