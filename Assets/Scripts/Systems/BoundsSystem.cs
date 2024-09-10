using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(MovementSystem))]
public partial class BoundsSystem : SystemBase
{
    private EntityQuery query;

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        const float lowestYPos = -5.0f;
        const float highestYPos = 15.0f; // For bullet

        Entities.WithAll<LocalToWorld>().ForEach((Entity entity, in LocalToWorld localToWorld) =>
        {
            float yPos = localToWorld.Position.y;
            if (yPos <= lowestYPos || yPos >= highestYPos)
            {
                ecb.DestroyEntity(entity);
            } 
        }).WithStructuralChanges().WithoutBurst().Run(); // Because we are making changes and accessing EntityManager

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
