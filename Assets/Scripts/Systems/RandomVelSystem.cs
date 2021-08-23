using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;
using Unity.Burst;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class RandomVelSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        Entities.WithNone<StarShipData>().ForEach((Entity e, int entityInQueryIndex, ref PhysicsVelocity PV)=> {
            Random rand = Random.CreateFromIndex((uint)entityInQueryIndex);
            PV.Linear = rand.NextFloat3(new float3(-1f,-1f,0), new float3(1f,1f,0));
        }).WithBurst().ScheduleParallel();
    }

    protected override void OnUpdate()
    {
        
    }
}
