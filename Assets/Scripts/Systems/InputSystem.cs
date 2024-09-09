using UnityEngine;
using Unity.Entities;
using UnityEngine.InputSystem;
using Unity.Mathematics;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial class InputSystem : SystemBase
{

    private InputActions inputActions;
    bool shooting;

    protected override void OnCreate()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.Shoot.performed += context => { shooting = true; };
        RequireForUpdate<PlayerControllerComponent>();
    }

    protected override void OnStartRunning() => inputActions.Enable();

    protected override void OnStopRunning() => inputActions.Disable();

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
