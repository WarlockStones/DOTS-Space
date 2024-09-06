using UnityEngine;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Mathematics;

public class EnemyAuthoring : MonoBehaviour
{
    public GameObject prefab;
}

public partial struct EnemyPrefabComponent : IComponentData
{
    public Entity value;
}

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        // Register prefab in the baker
        var entityPrefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);

        // Add Entity reference to a component from later instatiation
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new EnemyPrefabComponent() { value = entityPrefab });
    }
}
