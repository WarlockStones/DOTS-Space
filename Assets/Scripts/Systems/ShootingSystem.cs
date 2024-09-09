using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct ShootingSystem : ISystem
{
    float shootTimer;
    float shootCooldown;
    const float bulletSpeed = 10;
    Entity bullet;
    void OnCreate(ref SystemState state)
    {
        shootTimer = 0;
        shootCooldown = 0.1f;
        state.RequireForUpdate<PlayerControllerComponent>();
        state.RequireForUpdate<BulletPrefabComponent>();

        // TODO: Setup bullet
    }

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
                    state.EntityManager.AddComponent(bullet, typeof(VelocityComponent));
                    state.EntityManager.SetComponentData(bullet, new VelocityComponent(new float3(0, bulletSpeed, 0)));
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
