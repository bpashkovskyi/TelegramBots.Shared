namespace TelegramBots.Shared.UpdateHandlerAttributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AllowedUpdateTypeAttribute : UpdateHandlerAttribute
{
    public AllowedUpdateTypeAttribute(UpdateType updateType, string? pattern = null)
    {
        UpdateType = updateType;
        Pattern = pattern;
    }

    public UpdateType UpdateType { get; }

    public string? Pattern { get; }
}