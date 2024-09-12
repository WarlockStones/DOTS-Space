using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(VelocitySystem))]
public partial class BoundsSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        const float lowestYPos = -5.0f;

        Entities
            .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
            .WithAll<LocalToWorld>()
            .WithDisabled<IsDisabledTag>() 
            .WithAbsent<LifetimeComponent>()
            .ForEach(
            (Entity entity, EntityCommandBuffer ecb, in LocalToWorld localToWorld) =>
        {
            float yPos = localToWorld.Position.y;
            if (yPos <= lowestYPos)
            {
                // ecb.AddComponent(entity, new IsDisabledTag());
                ecb.SetComponentEnabled<IsDisabledTag>(entity, true);
                ecb.SetComponent(entity, LocalTransform.FromPosition(-999, -999, -999));
            }
        }).ScheduleParallel(); 
    }
}
