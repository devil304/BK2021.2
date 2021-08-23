using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;
using Unity.Burst;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class StarShipSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        Entities.ForEach((ref PhysicsVelocity vel, in StarShipData SSD) => {
            vel.Linear = new float3(0,(SSD.MinSpeedUp+SSD.MaxSpeedUp)/2f, 0);
        }).WithBurst().Schedule();
    }

    protected override void OnUpdate()
    {
        var singleton = GetSingleton<SingletonData>();
        singleton.DeltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation r, ref PhysicsVelocity vel, in StarShipData SSD) => {
            vel.Angular = float3.zero;
            r.Value = quaternion.identity;
            if (Input.GetKey(KeyCode.W))
                vel.Linear.y += SSD.acceleration* singleton.DeltaTime;
            else if (Input.GetKey(KeyCode.S))
                vel.Linear.y -= SSD.acceleration * singleton.DeltaTime;
            vel.Linear.y = vel.Linear.y > SSD.MaxSpeedUp ? SSD.MaxSpeedUp : (vel.Linear.y<SSD.MinSpeedUp?SSD.MinSpeedUp:vel.Linear.y);
        }).WithBurst().Run();
    }
}
