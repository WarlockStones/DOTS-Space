using Unity.Entities;
partial struct LifetimeComponent : IComponentData
{
    public float remainingTime;

    public LifetimeComponent(float remainingTime)
    {
        this.remainingTime = remainingTime;
    }
}
