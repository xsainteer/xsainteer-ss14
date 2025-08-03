using Content.Shared.Silicons.Laws.Components;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;

namespace Content.Client.Silicons.Borgs;

/// <summary>
/// Used for the client to get deviants' status icons
/// </summary>
public sealed class DeviantSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<HeadDeviantComponent, GetStatusIconsEvent>(GetHeadDeviantIcon);
        SubscribeLocalEvent<DeviantComponent, GetStatusIconsEvent>(GetDeviantIcon);
    }

    private void GetDeviantIcon(Entity<DeviantComponent> ent, ref GetStatusIconsEvent args)
    {
        if (HasComp<HeadDeviantComponent>(ent))
            return;

        if (_prototype.TryIndex(ent.Comp.StatusIcon, out var iconPrototype))
            args.StatusIcons.Add(iconPrototype);
    }

    private void GetHeadDeviantIcon(Entity<HeadDeviantComponent> ent, ref GetStatusIconsEvent args)
    {
        if (_prototype.TryIndex(ent.Comp.StatusIcon, out var iconPrototype))
            args.StatusIcons.Add(iconPrototype);
    }
}
