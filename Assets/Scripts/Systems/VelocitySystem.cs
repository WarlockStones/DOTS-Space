using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(TransformSystemGroup))]
partial struct VelocitySystem : ISystem
{
    EntityQuery query;

    [BurstCompile]
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
