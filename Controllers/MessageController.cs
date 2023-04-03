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
        _updatesBus = updatesBus;
        _rollbar = rollbar;
    }

    [HttpPost]
    public async Task<OkResult> HandleUpdateAsync(Update update)
    {
        try
        {
            await _updatesBus.SendAsync(update).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _rollbar.Critical(
                exception,
                new Dictionary<string, object?> { { "update", update } });
        }

        return Ok();
    }

    [HttpGet]
    public string Test()
    {
        return "It works";
    }
}