namespace TelegramBots.Shared.HealthCheck;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using Newtonsoft.Json;

public class WebHookHealthCheck : IHealthCheck
{
    private readonly string _apiKey;

    public WebHookHealthCheck(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var httpClient = new HttpClient();

            var uri = new Uri($"https://api.telegram.org/bot{_apiKey}/getWebhookInfo");

            var httpResponseMessage = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            var responseJson = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var getWebhookInfo = JsonConvert.DeserializeObject<GetWebhookInfo>(responseJson);

            if (getWebhookInfo == null || getWebhookInfo.Result == null)
            {
                return HealthCheckResult.Unhealthy(description: "Cannot get WebHook info");
            }

            var data = new Dictionary<string, object>
            {
                { "Ok", getWebhookInfo.Ok ?? false },
                { "Url", getWebhookInfo.Result.Url ?? string.Empty },
                { "PendingUpdateCount", getWebhookInfo.Result.PendingUpdateCount ?? default(int) },
                { "LastErrorMessage", getWebhookInfo.Result.LastErrorMessage ?? string.Empty },
            };

            var description = $"Ok: {getWebhookInfo.Ok}, Url: {getWebhookInfo.Result.Url}, PendingUpdateCount {getWebhookInfo.Result.PendingUpdateCount}, LastErrorMessage: {getWebhookInfo.Result.LastErrorMessage}";

            if (getWebhookInfo.Ok != true)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, description, data: data);
            }

            if (getWebhookInfo.Result.PendingUpdateCount > 0)
            {
                return HealthCheckResult.Unhealthy(description, data: data);
            }

            return HealthCheckResult.Healthy(description, data);
        }
        catch (DbException exception)
        {
            return HealthCheckResult.Unhealthy(exception: exception);
        }
    }
}