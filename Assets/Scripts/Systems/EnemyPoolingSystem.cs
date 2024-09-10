using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

public partial struct EnemyPoolingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        new PoolingJob()
        {
        }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct PoolingJob : IJobEntity
    {
        public void Execute()
        {

        }
    }
}
