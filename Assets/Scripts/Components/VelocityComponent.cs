using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct VelocityComponent : IComponentData
{
    public float3 velocity;

    public VelocityComponent(float3 velocity)
    {
        this.velocity = velocity;
    }
}
