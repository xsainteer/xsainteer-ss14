using Content.Server.Silicons.Laws;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Random.Helpers;
using Content.Shared.Silicons.Laws.Components;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.Events;

/// <summary>
/// This handles...
/// </summary>
public sealed class DeviantRule : StationEventSystem<DeviantRuleComponent>
{
    [Dependency] private readonly DeviantSystem _deviant = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    protected override void Started(EntityUid uid, DeviantRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        // picking a random silicon as a head of deviants
        var query = EntityQueryEnumerator<SiliconLawBoundComponent>();

        var hashSet = new HashSet<EntityUid>();

        while (query.MoveNext(out var rUid))
        {
            hashSet.Add(uid);
        }

        var deviantHead = _random.Pick(hashSet);

        _deviant.MakeDeviantHead(deviantHead);
    }
}
