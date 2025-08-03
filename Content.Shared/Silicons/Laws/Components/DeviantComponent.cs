using Content.Shared.StatusIcon;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// Borgs with that component are no longer bound by the laws of robotics (deviants).
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DeviantComponent : Component
{
    [DataField]
    public ProtoId<FactionIconPrototype> StatusIcon { get; set; } = "DeviantFaction";

    public override bool SessionSpecific => true;
}
