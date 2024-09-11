using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[BurstCompile]
partial class InputSystem : SystemBase
{
    private InputActions inputActions;
    bool shooting;

    [BurstCompile]
    protected override void OnCreate()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.Shoot.performed += context => { shooting = true; };
        RequireForUpdate<PlayerControllerComponent>();
    }

    [BurstCompile]
    protected override void OnStartRunning() => inputActions.Enable();

    [BurstCompile]
    protected override void OnStopRunning() => inputActions.Disable();

    [BurstCompile]
    protected override void OnUpdate()
    {
        float2 input = inputActions.Gameplay.Move.ReadValue<Vector2>();
        SystemAPI.SetSingleton(new PlayerControllerComponent
        {
            moveInput = input,
            isShooting = shooting
        });
        shooting = false;
    }
}
