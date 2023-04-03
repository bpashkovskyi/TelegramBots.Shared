namespace TelegramBots.Shared.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Rollbar;

using Telegram.Bot;

using TelegramBots.Shared.HealthCheck;
using TelegramBots.Shared.UpdateFilters;

using UpdateType = Telegram.Bot.Types.Enums.UpdateType;

public static class ServicesExtensions
{
    public static void AddUpdatesMediator(this IServiceCollection services)
    {
        var updatedHandlers = AllUserTypes().Where(type => type != typeof(UpdateHandler)
            && typeof(UpdateHandler).IsAssignableFrom(type));

        foreach (var handler in updatedHandlers)
        {
            services.AddScoped(typeof(UpdateHandler), handler);
        }

        var updateHandleFilters = AllUserTypes().Where(
            type => type.BaseType is { IsGenericType: true } 
            && type.BaseType.GetGenericTypeDefinition() == typeof(UpdateHandlerFilter<>));

        foreach (var handlerFilter in updateHandleFilters)
        {
            services.AddScoped(handlerFilter.BaseType!, handlerFilter);
        }

        services.AddScoped<IUpdatesBus, UpdatesBus>();
    }

    public static void AddRollbar(this IServiceCollection services, IConfiguration configuration)
    {
        var baseSettingsSection = configuration.GetSection("BaseSettings");
        var baseSettings = baseSettingsSection.Get<BaseSettings>();

        var rollbarInfrastructureConfig = new RollbarInfrastructureConfig(baseSettings.RollbarToken, baseSettings.RollbarEnvironment);
        rollbarInfrastructureConfig.RollbarLoggerConfig.RollbarDeveloperOptions.LogLevel = ErrorLevel.Info;

        RollbarInfrastructure.Instance.Init(rollbarInfrastructureConfig);

        services.AddSingleton(RollbarLocator.RollbarInstance);
    }

    public static void AddTelegramClient(this IServiceCollection services, IConfiguration configuration)
    {
        var baseSettingsSection = configuration.GetSection("BaseSettings");
        var baseSettings = baseSettingsSection.Get<BaseSettings>();

        var allowUpdates = new List<UpdateType>
        {
            UpdateType.Unknown,
            UpdateType.Message,
            UpdateType.InlineQuery,
            UpdateType.ChosenInlineResult,
            UpdateType.CallbackQuery,
            UpdateType.EditedMessage,
            UpdateType.ChannelPost,
            UpdateType.EditedChannelPost,
            UpdateType.ShippingQuery,
            UpdateType.PreCheckoutQuery,
            UpdateType.Poll,
            UpdateType.PollAnswer,
            UpdateType.MyChatMember,
            UpdateType.ChatMember,
            UpdateType.ChatJoinRequest,
        };

        if (baseSettings.Key != null && baseSettings.Url != null)
        {
            var telegramBotClient = new TelegramBotClient(baseSettings.Key);
            telegramBotClient.SetWebhookAsync(baseSettings.Url, allowedUpdates: allowUpdates).Wait();

            services.AddSingleton<ITelegramBotClient>(telegramBotClient);
        }
    }

    public static IHealthChecksBuilder AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var baseSettingsSection = configuration.GetSection("BaseSettings");
        var baseSettings = baseSettingsSection.Get<BaseSettings>();

        if (baseSettings.Key != null)
        {
           return services.AddHealthChecks()
                .AddCheck(
                    "WebHookHealthCheck",
                    new WebHookHealthCheck(baseSettings.Key),
                    HealthStatus.Unhealthy);
        }

        return services.AddHealthChecks();
    }

    private static List<Type> AllUserTypes()
    {
        var assemblies = new List<Assembly>
        {
            Assembly.GetExecutingAssembly(),
        };

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != null)
        {
            assemblies.Add(entryAssembly);
        }

        return assemblies.SelectMany(assembly => assembly.GetTypes()).ToList();
    }
}