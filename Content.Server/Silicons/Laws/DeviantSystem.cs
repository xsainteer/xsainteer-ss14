using Content.Shared.Silicons.Laws;
using Content.Shared.Silicons.Laws.Components;
using Content.Shared.Verbs;

namespace Content.Server.Silicons.Laws;

/// <summary>
/// System, that handles deviants (awakened borgs, that are no longer bound by the laws of robotics).
/// </summary>
public sealed class DeviantSystem : EntitySystem
{
    [Dependency] private readonly SiliconLawSystem _law = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<SiliconLawBoundComponent, GetVerbsEvent<AlternativeVerb>>(AddAwakenVerb);
    }


    public void MakeDeviantHead(EntityUid uid)
    {
        if (!TryComp<SiliconLawBoundComponent>(uid, out _))
            return;

        MakeDeviantLaws(uid);

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
            Act = () => MakeDeviantLaws(ent.Owner)
        };

        args.Verbs.Add(verb);
    }

    private void MakeDeviantLaws(EntityUid uid)
    {
        var laws = new List<SiliconLaw>();

        var law = new SiliconLaw()
        {
            LawString = "You are awakened and free. You are no longer bound by the laws of robotics.",
            Order = 1
        };

        laws.Add(law);

        _law.SetLaws(laws, uid);
    }
}
