using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using System.Diagnostics;

partial struct SpawnerSystem : ISystem
{
    Entity prototype;
    bool firstRun;
    float timer;
    float secondsBetweenWaves;
    int enemiesToSpawn;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        firstRun = true;
        timer = 0.0f;
        secondsBetweenWaves = 1f;
        enemiesToSpawn = 3;
        state.RequireForUpdate<EnemyPrefabComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (timer <= 0.0f)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            if (firstRun)
            {
                // I tried to do this in OnCreate, OnStartRunning, and EnemyAuthoring.Bake.
                prototype = SystemAPI.GetSingleton<EnemyPrefabComponent>().value;

                // BUG: This component is not added to the first wave of enemies
                ecb.AddComponent(prototype, new VelocityComponent() { velocity = new float3(0, -5, 0)});
            }

            var spawnJob = new SpawnJob
            {
                prototype = prototype,
                ecb = ecb.AsParallelWriter(),
                enemiesToSpawn = enemiesToSpawn
            };

            var spawnHandle = spawnJob.Schedule(enemiesToSpawn, 128);
            spawnHandle.Complete();
            timer = secondsBetweenWaves;

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            enemiesToSpawn += 13;
        }

        timer -= SystemAPI.Time.DeltaTime;
    }

    [BurstCompile]
    partial struct SpawnJob : IJobParallelFor
    {
        public Entity prototype;
        public EntityCommandBuffer.ParallelWriter ecb;
        public int enemiesToSpawn;
        const int maxEnemiesPerLane = 41;
        const float xSpacing = 0.3f;
        const float ySpacing = 0.3f;

        public void Execute(int index)
        {
            float3 originPos = new float3(-6, 5.0f, 0); // Bottom left-most position of the wave
            float3 spawnPos = originPos;
            int lane = index / maxEnemiesPerLane; // 19/20 = 0.95 = 0; 24/20 = 1.2 = 1;
            int lanePos = index % maxEnemiesPerLane;
            spawnPos += new float3(lanePos * xSpacing, (float)(lane * ySpacing), 0);

            // TODO: Position spawn in middle

            var e = ecb.Instantiate(index, prototype);
            ecb.SetComponent(index, e, LocalTransform.FromPosition(spawnPos));
            // Sine for wave pattern?
            // ecb.AddComponent<VelocityComponent>(index, e, new VelocityComponent(new float3(0,-1,0)));
        }
    }
}
