using Unity.Entities;
using Unity.Mathematics;

// This system should not be multithreaded!
partial struct PlayerMovementSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerControllerComponent>();
    }

    void OnUpdate(ref SystemState state)
    {
        var player = SystemAPI.GetSingletonEntity<PlayerControllerComponent>();
        const float moveSpeed = 15;

        float2 moveInput = SystemAPI.GetSingleton<PlayerControllerComponent>().moveInput;
        float3 newMovement = new float3(moveInput.x, moveInput.y, 0) * moveSpeed;
        VelocityComponent newVelocity = new(newMovement);

        state.EntityManager.SetComponentData(player, newVelocity);
    }
}
