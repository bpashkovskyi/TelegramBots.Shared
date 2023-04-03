namespace TelegramBots.Shared.HealthCheck;

using Newtonsoft.Json;

internal class GetWebhookInfo
{
    [JsonProperty("ok")]
    public bool? Ok { get; set; }

    [JsonProperty("result")]
    public Result? Result { get; set; }
}