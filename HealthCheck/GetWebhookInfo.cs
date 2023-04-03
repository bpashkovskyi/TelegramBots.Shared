using Newtonsoft.Json;

namespace TelegramBots.Shared.HealthCheck;

internal class GetWebhookInfo
{
    [JsonProperty("ok")]
    public bool? Ok { get; set; }

    [JsonProperty("result")]
    public Result? Result { get; set; }
}