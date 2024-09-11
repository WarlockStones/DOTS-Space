using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public partial struct ShootingSystem : ISystem
{
    float shootTimer;
    float shootCooldown;
    const float bulletSpeed = 10;
    const float bulletLifetime = 3;
    Entity bullet;

    [BurstCompile]
    void OnCreate(ref SystemState state)
    {
        shootTimer = 0;
        shootCooldown = 0.1f;
        state.RequireForUpdate<PlayerControllerComponent>();
        state.RequireForUpdate<BulletPrefabComponent>();
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {

        if (shootTimer <= 0)
        {
            bool isShooting = SystemAPI.GetSingleton<PlayerControllerComponent>().isShooting;
            if (isShooting)
            {
                if (bullet == Entity.Null) 
                {
                    bullet = SystemAPI.GetSingleton<BulletPrefabComponent>().value;
                    state.EntityManager.AddComponent<VelocityComponent>(bullet);
                    state.EntityManager.SetComponentData(bullet, new VelocityComponent(new float3(0, bulletSpeed, 0)));
                    state.EntityManager.AddComponent<IsDisabledTag>(bullet);
                    state.EntityManager.SetComponentEnabled<IsDisabledTag>(bullet, false);
                    state.EntityManager.AddComponent<LifetimeComponent>(bullet);
                    state.EntityManager.SetComponentData(bullet, new LifetimeComponent(bulletLifetime));
                }

                var player = SystemAPI.GetSingletonEntity<PlayerControllerComponent>();
                var playerPos = SystemAPI.GetComponentRO<LocalTransform>(player).ValueRO.Position;

                var localToWorld = state.EntityManager.GetComponentData<LocalToWorld>(player);
                var instance = state.EntityManager.Instantiate(bullet);
                state.EntityManager.SetComponentData(instance, LocalTransform.FromPosition(playerPos));
                shootTimer = shootCooldown;
            }
        }
        shootTimer -= SystemAPI.Time.DeltaTime;
    }
}
