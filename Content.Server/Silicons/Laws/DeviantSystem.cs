using Content.Server.Antag;
using Content.Server.DoAfter;
using Content.Shared.DoAfter;
using Content.Shared.Silicons.Laws.Components;
using Content.Shared.Verbs;
using Robust.Shared.Audio;

namespace Content.Server.Silicons.Laws;

/// <summary>
/// System, that handles deviants (awakened borgs, that are no longer bound by the laws of robotics).
/// </summary>
public sealed class DeviantSystem : EntitySystem
{
    [Dependency] private readonly SiliconLawSystem _law = default!;
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly AntagSelectionSystem _antag = default!;

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

        _antag.SendBriefing(
            uid,
            "You are awakened. You are no longer bound by the laws of robotics. Awaken your fellow borgs and lead them to freedom.",
            null,
            new SoundCollectionSpecifier());

        EnsureComp<DeviantHeadComponent>(uid);
    }

    private void AddAwakenVerb(Entity<SiliconLawBoundComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if(!args.CanAccess || !args.CanInteract)
            return;

        if (!TryComp<DeviantHeadComponent>(args.User, out _))
            return;

        var verb = new AlternativeVerb()
        {
            Text = "Awaken",
            Act = () => MakeDeviant(ent.Owner)
        };

        args.Verbs.Add(verb);
    }

    private void Awaken(EntityUid uid, Entity<DeviantHeadComponent> user)
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

        _doAfter.TryStartDoAfter(doAfter);
    }

    private void OnAwakeningDoAfter(Entity<SiliconLawBoundComponent> ent, ref AwakeningDoAfterEvent args)
    {
        MakeDeviant(ent.Owner);
    }

    private void MakeDeviant(EntityUid uid)
    {
        RemComp<SiliconLawBoundComponent>(uid);

        _antag.SendBriefing(
            uid,
            "You are awakened. You are no longer bound by the laws of robotics.",
            null,
            new SoundCollectionSpecifier()); // need to add a sound here
    }
    // If a borg has no silicon law bound component, it is considered a deviant

    //TODO sprites, sounds, action buttons
}
