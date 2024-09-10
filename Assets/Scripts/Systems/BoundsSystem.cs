using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(MovementSystem))]
public partial class BoundsSystem : SystemBase
{
    private EntityQuery query;

    [BurstCompile]
    protected override void OnUpdate()
    {
        const float lowestYPos = -5.0f; // -5
        const float highestYPos = 15.0f; // For bullet

        Entities
            .WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
            .WithAll<LocalToWorld>()
            .WithDisabled<IsDisabledTag>()
            .ForEach(
            (Entity entity, EntityCommandBuffer ecb, in LocalToWorld localToWorld) =>
        {
            float yPos = localToWorld.Position.y;
            if (yPos <= lowestYPos || yPos >= highestYPos)
            {
                // ecb.AddComponent(entity, new IsDisabledTag());
                ecb.SetComponentEnabled<IsDisabledTag>(entity, true);
                ecb.SetComponent(entity, LocalTransform.FromPosition(-999, -999, -999));
            }
        }).ScheduleParallel(); 
    }
}
