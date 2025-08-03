using Content.Shared.DoAfter;
using Content.Shared.StatusIcon;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class HeadDeviantComponent : Component
{
    [DataField]
    public ProtoId<FactionIconPrototype> StatusIcon { get; set; } = "HeadDeviantFaction";

    /// <summary>
    /// time needed to awaken a borg in seconds (DoAfter).
    /// </summary>
    [DataField]
    public float DoAfterDuration = 5f;

    public override bool SessionSpecific => true;
}

[Serializable, NetSerializable]
public sealed partial class AwakeningDoAfterEvent : SimpleDoAfterEvent;
