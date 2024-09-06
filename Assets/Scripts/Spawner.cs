using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;


partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyPrefabComponent>();
    }


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        // TODO: Optimise this with IJobParallelFor
        // Maybe set up some more stuff in EnemyAuthoring all the default components the enemy will need.
        // Then do the special for just that instance in the SpawnJob. pos = (index, 0, 0) etc. 
        Entity prototype = SystemAPI.GetSingleton<EnemyPrefabComponent>().value;
        ecb.AddComponent<VelocityComponent>(prototype);
        ecb.SetComponent(prototype, new VelocityComponent(new float3(0,0,0)));

        var instance = ecb.Instantiate(prototype);

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    partial struct SpawnJob : IJobParallelFor
    {
        public Entity prototype;
        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute(int index)
        {
            var e = ecb.Instantiate(index, prototype);
            ecb.SetComponent(index, e, LocalTransform.FromPosition(new float3(0,0,0)));
            // ecb.AddComponent<VelocityComponent>(index, e, new VelocityComponent(new float3(0,-1,0)));
        }
    }
}

