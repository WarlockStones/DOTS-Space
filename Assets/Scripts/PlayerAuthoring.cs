using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public class PlayerAuthoring : MonoBehaviour
{
    public int maxHealth = 10;
    public float movementSpeed = 5.0f;
}

[BurstCompile]
class PlayerBaker : Baker<PlayerAuthoring>
{

    [BurstCompile]
    public override void Bake(PlayerAuthoring authoring)
    {
        var playerEntity = GetEntity(TransformUsageFlags.Dynamic);

        float movementSpeed = authoring.movementSpeed;
        AddComponent(playerEntity, new PlayerControllerComponent{});
        AddComponent(playerEntity, new VelocityComponent());
        AddComponent(playerEntity, new IsDisabledTag());
        SetComponentEnabled<IsDisabledTag>(playerEntity, false);
    }
}
