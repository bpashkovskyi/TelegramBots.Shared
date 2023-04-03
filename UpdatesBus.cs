namespace TelegramBots.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Rollbar;

using Telegram.Bot.Types;

using TelegramBots.Shared.UpdateFilters;
using TelegramBots.Shared.UpdateHandlerAttributes;

public sealed class UpdatesBus : IUpdatesBus
{
    private readonly IRollbar rollbar;
    private readonly IServiceProvider serviceProvider;

    public UpdatesBus(IRollbar rollbar, IServiceProvider serviceProvider)
    {
        this.rollbar = rollbar;
        this.serviceProvider = serviceProvider;
    }

    public async Task SendAsync(Update update)
    {
        this.rollbar.Debug("Starting finding handlers");

        var updateHandlers = await this.FilterHandlersAsync(update).ConfigureAwait(false);

        if (!updateHandlers.Any())
        {
            this.rollbar.Debug("No handlers found");
            return;
        }

        foreach (var updateHandler in updateHandlers.OrderBy(updateHandler => updateHandler.Order))
        {
            this.rollbar.Debug($"Found handler {updateHandler}");

            this.SetHandlerArguments(updateHandler, update);
            await updateHandler.HandleAsync(update).ConfigureAwait(false);
        }
    }

    private async Task<List<UpdateHandler>> FilterHandlersAsync(Update update)
    {
        var updateHandlers = this.serviceProvider.GetServices<UpdateHandler>().ToList();
        var filteredUpdateHandlers = new List<UpdateHandler>();

        foreach (var updateHandler in updateHandlers)
        {
            if (await this.MatchFilters(updateHandler, update).ConfigureAwait(false))
            {
                filteredUpdateHandlers.Add(updateHandler);
            }
        }

        return filteredUpdateHandlers;
    }

    private async Task<bool> MatchFilters(UpdateHandler updateHandler, Update update)
    {
        var updateHandlerAttributes = updateHandler.GetType().GetCustomAttributes(typeof(UpdateHandlerAttribute), true)
            .Cast<UpdateHandlerAttribute>();

        foreach (var updateHandlerAttribute in updateHandlerAttributes)
        {
            var updateHandlerFilterType = typeof(UpdateHandlerFilter<>).MakeGenericType(updateHandlerAttribute.GetType());

            var updateHandlerFilter = (dynamic)this.serviceProvider.GetRequiredService(updateHandlerFilterType);

            var updateFilterMatch = (bool)updateHandlerFilter.Matches((dynamic)updateHandlerAttribute, update) || await ((Task<bool>)updateHandlerFilter.MatchesAsync((dynamic)updateHandlerAttribute, update)).ConfigureAwait(false);

            if (!updateFilterMatch)
            {

                return false;
            }
        }

        return true;
    }

    private void SetHandlerArguments(UpdateHandler updateHandler, Update update)
    {
        var updateHandlerAttributes = updateHandler.GetType().GetCustomAttributes(typeof(UpdateHandlerAttribute), true)
            .Cast<UpdateHandlerAttribute>();

        foreach (var updateHandlerAttribute in updateHandlerAttributes)
        {
            var updateHandlerFilterType = typeof(UpdateHandlerFilter<>).MakeGenericType(updateHandlerAttribute.GetType());

            var updateHandlerFilter = (dynamic)this.serviceProvider.GetRequiredService(updateHandlerFilterType);
            updateHandlerFilter.SetHandlerArguments((dynamic)updateHandlerAttribute, updateHandler, update);
        }
    }
}