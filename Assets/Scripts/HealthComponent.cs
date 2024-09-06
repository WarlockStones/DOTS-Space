using Unity.Entities;

public struct HealthComponent : IComponentData
{
    public int maxHealth;
    public int currentHealth;
}
