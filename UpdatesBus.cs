using Microsoft.Extensions.DependencyInjection;

using Rollbar;

using TelegramBots.Shared.UpdateFilters;

namespace TelegramBots.Shared;

public sealed class UpdatesBus : IUpdatesBus
{
    private readonly IRollbar _rollbar;
    private readonly IServiceProvider _serviceProvider;

    public UpdatesBus(IRollbar rollbar, IServiceProvider serviceProvider)
    {
        _rollbar = rollbar;
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync(Update update)
    {
        _rollbar.Debug("Starting finding handlers");

        var updateHandlers = await FilterHandlersAsync(update).ConfigureAwait(false);

        if (!updateHandlers.Any())
        {
            _rollbar.Debug("No handlers found");
            return;
        }

        foreach (var updateHandler in updateHandlers.OrderBy(updateHandler => updateHandler.Order))
        {
            _rollbar.Debug($"Found handler {updateHandler}");

            SetHandlerArguments(updateHandler, update);
            await updateHandler.HandleAsync(update).ConfigureAwait(false);
        }
    }

    private async Task<List<UpdateHandler>> FilterHandlersAsync(Update update)
    {
        var updateHandlers = _serviceProvider.GetServices<UpdateHandler>().ToList();
        var filteredUpdateHandlers = new List<UpdateHandler>();

        foreach (var updateHandler in updateHandlers)
        {
            if (await MatchFilters(updateHandler, update).ConfigureAwait(false))
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

            var updateHandlerFilter = (dynamic)_serviceProvider.GetRequiredService(updateHandlerFilterType);

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

            var updateHandlerFilter = (dynamic)_serviceProvider.GetRequiredService(updateHandlerFilterType);
            updateHandlerFilter.SetHandlerArguments((dynamic)updateHandlerAttribute, updateHandler, update);
        }
    }
}