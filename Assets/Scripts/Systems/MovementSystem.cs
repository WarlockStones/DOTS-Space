using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformSystemGroup))]
partial struct MovementSystem : ISystem
{
    EntityQuery query;

    void OnCreate(ref SystemState state)
    {
        query = new EntityQueryBuilder(Allocator.Temp)
            .WithAllRW<LocalTransform>()
            .WithAll<VelocityComponent>()
            .WithDisabled<IsDisabledTag>()
            .Build(ref state);
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        new MovementJob() { 
            deltaTime = SystemAPI.Time.DeltaTime 
        }.ScheduleParallel(query);
    }
}

[BurstCompile]
partial struct MovementJob : IJobEntity
{
    public float deltaTime;
    public void Execute(RefRW<LocalTransform> transform, RefRO<VelocityComponent> velocityComp)
    {
        transform.ValueRW.Position += (velocityComp.ValueRO.velocity) * deltaTime;
    }
}


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
