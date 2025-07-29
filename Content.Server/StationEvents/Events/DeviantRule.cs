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

        if (!TryGetRandomStation(out var chosenStation))
            return;

        var query = EntityQueryEnumerator<SiliconLawBoundComponent, TransformComponent, IonStormTargetComponent>();

        var hashSet = new HashSet<EntityUid>();

        while (query.MoveNext(out var rUid, out _, out _, out _))
        {
            hashSet.Add(uid);
        }

        var deviantHead = _random.Pick(hashSet);


    }
}
