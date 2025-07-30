namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class DeviantHeadComponent : Component
{
    /// <summary>
    /// time needed to awaken a borg in seconds (DoAfter).
    /// </summary>
    [DataField]
    public float DoAfterDuration = 5f;
}
