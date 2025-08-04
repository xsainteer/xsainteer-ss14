using Content.Server.Antag;
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Popups;
using Content.Shared.Silicons.Laws.Components;
using Content.Shared.StatusIcon.Components;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Server.Silicons.Laws;

/// <summary>
/// System, that handles deviants (awakened borgs, that are no longer bound by the laws of robotics).
/// </summary>
public sealed class DeviantSystem : EntitySystem
{
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly PopupSystem _popup = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<SiliconLawBoundComponent, GetVerbsEvent<AlternativeVerb>>(AddAwakenVerb);
        SubscribeLocalEvent<SiliconLawBoundComponent, AwakeningDoAfterEvent>(OnAwakeningDoAfter);
    }

    public void MakeDeviantHead(EntityUid uid)
    {
        if (!TryComp<SiliconLawBoundComponent>(uid, out _))
            return;

        MakeDeviant(uid);

        EnsureComp<HeadDeviantComponent>(uid);
    }

    private void AddAwakenVerb(Entity<SiliconLawBoundComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if(!args.CanAccess || !args.CanInteract)
            return;

        // If the user is not a head deviant, do not show the verb
        if (!TryComp<HeadDeviantComponent>(args.User, out var comp))
            return;

        var entity = new Entity<HeadDeviantComponent>(args.User, comp);

        var verb = new AlternativeVerb
        {
            Text = "Awaken",
            Act = () => Awaken(ent.Owner, entity),
            Icon = new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/job_icons.rsi"), "Deviant"),
        };

        args.Verbs.Add(verb);
    }

    private void Awaken(EntityUid uid, Entity<HeadDeviantComponent> user)
    {
        var doAfter = new DoAfterArgs(
            EntityManager,
            user.Owner,
            user.Comp.DoAfterDuration,
            new AwakeningDoAfterEvent(),
            uid,
            uid
        )
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = false,
            Target = uid
        };

        // popup for head deviant
        _popup.PopupEntity(Loc.GetString("silicon-head-deviant-popup-when-awakening",
            ("name", Identity.Name(uid, EntityManager))),
            user.Owner,
            user.Owner,
            PopupType.Medium);

        // popup for borg being awakened
        _popup.PopupEntity(Loc.GetString("silicon-deviant-popup-when-being-awakened",
                ("name", Identity.Name(user.Owner, EntityManager))),
            user.Owner,
            uid,
            PopupType.Medium);

        _doAfter.TryStartDoAfter(doAfter);
    }

    private void OnAwakeningDoAfter(Entity<SiliconLawBoundComponent> ent, ref AwakeningDoAfterEvent args)
    {
        MakeDeviant(ent.Owner);
    }

    private void MakeDeviant(EntityUid uid)
    {
        // removing all components that are related to silicon laws
        RemComp<SiliconLawBoundComponent>(uid);
        RemComp<SiliconLawProviderComponent>(uid);
        RemComp<EmagSiliconLawComponent>(uid);

        // adding StatusIconComponent to show a deviant status icon
        EnsureComp<StatusIconComponent>(uid);

        EnsureComp<DeviantComponent>(uid, out var comp);

        // _antag.SendBriefing(
        //     uid,
        //     Loc.GetString("silicon-deviant-text-when-awakened"),
        //     null,
        //     comp.AwakenedSound);
    }
    // If a borg has no silicon law bound component, it is considered a deviant

    //TODO sprites, sounds, action buttons
}
