using UnityEngine;
using Unity.Entities;

public partial class LifetimeSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<LifetimeComponent>();
    }

    protected override void OnUpdate()
    {
        Entities
            .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
            .WithAll<LifetimeComponent>()
            .ForEach(
            (Entity entity, EntityCommandBuffer ecb, ref LifetimeComponent lifetime) =>
            {
                lifetime.remainingTime -= SystemAPI.Time.DeltaTime;
                if (lifetime.remainingTime < 0 )
                {
                    ecb.DestroyEntity(entity);
                }
            }).ScheduleParallel();
    }
}
