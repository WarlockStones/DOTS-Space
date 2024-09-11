using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[BurstCompile]
partial struct SpawnerSystem : ISystem
{
    Entity prototype;
    EntityQuery query;
    float timer;
    float secondsBetweenWaves;
    int enemiesToSpawn;

    [BurstCompile]
    void OnCreate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        timer = 0.0f;
        secondsBetweenWaves = 1f;
        enemiesToSpawn = 3;

        query = new EntityQueryBuilder(Allocator.Temp)
            .WithAllRW<LocalToWorld>()
            .WithAll<IsDisabledTag>()
            .Build(ref state);

        state.RequireForUpdate<IsDisabledTag>();
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        if (timer <= 0.0f)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = query.ToEntityArray(Allocator.Temp);
            if (entities.Length < enemiesToSpawn)
            {
                enemiesToSpawn = entities.Length;
            }

            for (int i = 0; i < enemiesToSpawn; ++i)
            {
                const int maxEnemiesPerLane = 41;
                const float xSpacing = 0.3f;
                const float ySpacing = 0.3f;

                float3 originPos = new float3(-6, 5.0f, 0); // Bottom left-most position of the wave
                float3 spawnPos = originPos;
                int lane = i / maxEnemiesPerLane; // 19/20 = 0.95 = 0; 24/20 = 1.2 = 1;
                int lanePos = i % maxEnemiesPerLane;
                spawnPos += new float3(lanePos * xSpacing, (float)(lane * ySpacing), 0);

                var e = entities[i];
                ecb.SetComponent(e, LocalTransform.FromPosition(spawnPos));
                ecb.SetComponentEnabled<IsDisabledTag>(e, false);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            timer = secondsBetweenWaves;
            enemiesToSpawn += 25;
        }

        timer -= SystemAPI.Time.DeltaTime;
    }
}
