using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public partial class LifetimeSystem : SystemBase
{
    [BurstCompile]
    protected override void OnCreate()
    {
        RequireForUpdate<LifetimeComponent>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Entities
            .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
            .WithAll<LifetimeComponent>()
            .ForEach(
            (Entity entity, EntityCommandBuffer ecb, ref LifetimeComponent lifetime) =>
            {
                lifetime.remainingTime -= SystemAPI.Time.DeltaTime;
                if (lifetime.remainingTime < 0)
                {
                    ecb.DestroyEntity(entity);
                }
            }).ScheduleParallel();
    }
}
