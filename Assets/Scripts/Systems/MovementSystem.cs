using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformSystemGroup))]
partial struct MovementSystem : ISystem
{
    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        // Use query automatically generated from matching parameters in Job's Execute
        new MovementJob() { 
            deltaTime = SystemAPI.Time.DeltaTime 
        }.ScheduleParallel();
    }
}

[BurstCompile]
partial struct MovementJob : IJobEntity
{
    public float deltaTime;
    public void Execute(RefRW<LocalTransform> transform, RefRO<VelocityComponent> velocityComp)
    {
        // TODO: How to: SystemAPI.Time.DeltaTime;
        transform.ValueRW.Position += (velocityComp.ValueRO.velocity) * deltaTime;
    }
}

/* Manual Query 
   EntityQuery query; 
   OnCreate:
     query = GetEntityQuery(ComponentType.ReadWrite<SampleComponent>(),
                            ComponentType.ReadOnly<BoidTarget>());
   OnUpdate:
     new MovementJob().ScheduleParallel(query)
     
 */
     

/* Method 1 not using Burst + Multithreading
public void OnUpdate(ref SystemState state)
{
    var deltaTime = SystemAPI.Time.DeltaTime;
    // TODO: Get all entities with Transform and VelocityComponent

    foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<VelocityComponent>())
    {
        float3 f = new float3(0, -1, 0);
        transform.ValueRW.Position += f * deltaTime;
    }
*/
