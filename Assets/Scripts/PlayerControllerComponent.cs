using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public struct PlayerControllerComponent : IComponentData
{
    public float2 moveInput;
    public bool isShooting;
}
