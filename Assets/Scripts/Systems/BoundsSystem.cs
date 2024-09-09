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
public partial class BoundsSystem : SystemBase
{
    private EntityQuery query;

    protected override void OnUpdate()
    {
        const float lowestYPos = -5.0f;
        Entities.WithAll<LocalToWorld>().ForEach((Entity entity, in LocalToWorld localToWorld) =>
        {
            if (localToWorld.Position.y <= lowestYPos)
            {
                EntityManager.DestroyEntity(entity);
            }
        }).WithStructuralChanges().WithoutBurst().Run(); // Because we are making changes and accessing EntityManager
    }
}
