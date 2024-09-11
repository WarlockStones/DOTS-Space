using Unity.Entities;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public class BulletAuthoring : MonoBehaviour
{
    public GameObject prefab;
}

[BurstCompile]
public partial struct BulletPrefabComponent : IComponentData
{
    public Entity value;
}

[BurstCompile]
public class BulletBaker : Baker<BulletAuthoring>
{
    [BurstCompile]
    public override void Bake(BulletAuthoring authoring)
    {
        var prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);

        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new BulletPrefabComponent() { value = prefab });
    }
}
