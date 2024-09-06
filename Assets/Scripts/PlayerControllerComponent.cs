using Unity.Entities;
using Unity.Mathematics;

public struct PlayerControllerComponent : IComponentData
{
    public float2 moveInput;

    // public bool isShooting;
    // Then on some Shoot System maybe even in PlayerMovement to PlayerInputActions
    // if isShooting && shootTimer < 0; Shoot();
}
