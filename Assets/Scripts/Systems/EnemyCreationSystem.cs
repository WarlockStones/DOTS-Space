using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;

public partial struct EnemyCreationTag : IComponentData { };

public partial struct EnemyCreationSystem : ISystem
{
    const int enemiesToCreate = 3072; 

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyPrefabComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entity prototype = SystemAPI.GetSingleton<EnemyPrefabComponent>().value;
        ecb.AddComponent(prototype, new VelocityComponent() 
        { 
            velocity = new float3(0, -5, 0)
        });
        ecb.AddComponent(prototype, new IsDisabledTag());
        ecb.SetComponentEnabled<IsDisabledTag>(prototype, true);
        ecb.SetComponent(prototype, LocalTransform.FromPosition(-999, -999, -999));
        // Update the prototype to have correct components
        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        // Create a new buffer for instatiating enemies
        ecb = new EntityCommandBuffer(Allocator.TempJob);

        var job = new CreationJob()
        {
            prototype = prototype,
            ecb = ecb.AsParallelWriter()
        };
        JobHandle handle = job.Schedule(enemiesToCreate, 128);
        handle.Complete();

        // TODO: Decide on what to do here
        // Find and remove EnemyCreationTag when done
        // Just use EnemyPrefab thing?
        ecb.DestroyEntity(SystemAPI.GetSingletonEntity<EnemyPrefabComponent>());

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    partial struct CreationJob : IJobParallelFor
    {
        public Entity prototype;
        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute(int index)
        {
            ecb.Instantiate(index, prototype);
        }
    }
}
