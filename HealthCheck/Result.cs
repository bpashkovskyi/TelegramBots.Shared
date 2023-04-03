namespace TelegramBots.Shared.HealthCheck;

using Newtonsoft.Json;

internal class Result
{
    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("pending_update_count")]
    public int? PendingUpdateCount { get; set; }

    [JsonProperty("last_error_message")]
    public string? LastErrorMessage { get; set; }
}