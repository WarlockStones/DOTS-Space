using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using System.ComponentModel;
using Unity.Collections;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(MovementSystem))]
public partial struct BoundsSystem : ISystem
{
    private EntityQuery query;

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

        // TODO: Limit player movement

        // TODO: Job
        foreach ((LocalToWorld localToWorld, Entity entity) in SystemAPI.Query<LocalToWorld>().WithEntityAccess())
        {
            if (localToWorld.Position.y <= -5)
            {
                ecb.DestroyEntity(entity);
                // ecb.RemoveComponent(entity, VelocityComponent);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
