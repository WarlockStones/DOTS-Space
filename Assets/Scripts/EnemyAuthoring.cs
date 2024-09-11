using UnityEngine;
using Unity.Entities;
using Unity.Burst;

[BurstCompile]
public class EnemyAuthoring : MonoBehaviour
{
    public GameObject prefab;
}

[BurstCompile]
public partial struct EnemyPrefabComponent : IComponentData
{
    public Entity value;
}

[BurstCompile]
public class EnemyBaker : Baker<EnemyAuthoring>
{
    [BurstCompile]
    public override void Bake(EnemyAuthoring authoring)
    {
        // Register prefab in the baker
        var entityPrefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);

        // Add prefab to empty entitiy's component from later instatiation
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new EnemyPrefabComponent() { value = entityPrefab });
        AddComponent(entity, new EnemyCreationTag()); // Tells EnemyCreationSystem it is ready to run
    }
}
