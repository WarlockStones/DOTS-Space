using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public GameObject prefab;
}

public partial struct BulletPrefabComponent : IComponentData
{
    public Entity value;
}

public class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        var prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);

        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new BulletPrefabComponent() { value = prefab });
    }
}
