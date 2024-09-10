using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public int maxHealth = 10;
    public float movementSpeed = 5.0f;
}

class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(playerEntity, new HealthComponent
        {
            maxHealth = authoring.maxHealth,
            currentHealth = authoring.maxHealth
        });

        float movementSpeed = authoring.movementSpeed;
        AddComponent(playerEntity, new PlayerControllerComponent{});
        AddComponent(playerEntity, new VelocityComponent());
        AddComponent(playerEntity, new IsDisabledTag());
        SetComponentEnabled<IsDisabledTag>(playerEntity, false);
    }
}
