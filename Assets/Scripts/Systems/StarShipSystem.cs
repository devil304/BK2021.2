using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;
using Unity.Burst;
using Unity.Collections;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class StarShipSystem : SystemBase
{

    protected override void OnStartRunning()
    {
        var singleton = GetSingleton<SingletonData>();
        Entities.ForEach((ref PhysicsVelocity vel, in StarShipData SSD) => {
            vel.Linear = new float3(0,(SSD.MinSpeedUp+SSD.MaxSpeedUp)/2f, 0);
            singleton.maxX = SSD.maxX;
            singleton.maxY = SSD.maxY;
        }).WithBurst().Run();
        SetSingleton(singleton);
    }

    protected override void OnUpdate()
    {
        var singleton = GetSingleton<SingletonData>();
        singleton.DeltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation r, ref PhysicsVelocity vel, in StarShipData SSD, in Translation t) => {
            vel.Angular = float3.zero;
            r.Value = quaternion.identity;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                vel.Linear.y += SSD.acceleration* singleton.DeltaTime;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                vel.Linear.y -= SSD.acceleration * singleton.DeltaTime;
            vel.Linear.y = vel.Linear.y > SSD.MaxSpeedUp ? SSD.MaxSpeedUp : (vel.Linear.y<SSD.MinSpeedUp?SSD.MinSpeedUp:vel.Linear.y);

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                vel.Linear.x = SSD.speedSides;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                vel.Linear.x = -SSD.speedSides;
            else
                vel.Linear.x = 0;
            singleton.PlayerPos = t.Value;
        }).WithBurst().Run();
        Dependency.Complete();
        SetSingleton(singleton);

        Entities.ForEach((in ExchangeData ED) => {
            ED.EMDE.SD = singleton;
        }).WithoutBurst().Run();
    }
}
