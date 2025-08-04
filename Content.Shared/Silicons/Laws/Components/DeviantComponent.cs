using Content.Shared.StatusIcon;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// Borgs with that component are no longer bound by the laws of robotics (deviants).
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DeviantComponent : Component
{
    /// <summary>
    /// Icon that is displayed on the borg's HUD when it is a deviant.
    /// </summary>
    [DataField]
    public ProtoId<FactionIconPrototype> StatusIcon { get; set; } = "DeviantFaction";

    /// <summary>
    /// Sound that plays when the borg is awakened.
    /// </summary>
    [DataField]
    public SoundSpecifier AwakenedSound = new SoundPathSpecifier("/Audio/Ambience/Antag/deviant_awakened.ogg");

    public override bool SessionSpecific => true;
}
