using Unity.Entities;
using Unity.Burst;

[BurstCompile]
partial struct LifetimeComponent : IComponentData
{
    public float remainingTime;

    public LifetimeComponent(float remainingTime)
    {
        this.remainingTime = remainingTime;
    }
}
