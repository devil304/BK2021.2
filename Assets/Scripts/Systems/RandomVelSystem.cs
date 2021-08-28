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
        Entities.WithAll<AsteroidTag>().ForEach((Entity e, int entityInQueryIndex, ref PhysicsVelocity PV)=> {
            Random rand = Random.CreateFromIndex((uint)entityInQueryIndex);
            PV.Linear = rand.NextFloat3(new float3(-1f,-1f,0), new float3(1f,1f,0));
        }).WithBurst().ScheduleParallel();
    }

    protected override void OnUpdate()
    {
        var singleton = GetSingleton<SingletonData>();
        Entities.WithAll<AsteroidTag>().ForEach((Entity e, int entityInQueryIndex, ref PhysicsVelocity PV, in Translation t) => {
            if (t.Value.x > singleton.maxX || t.Value.x < -singleton.maxX || t.Value.y > singleton.maxY || t.Value.y < -singleton.maxY)
            {
                Random rand = new Random((uint)entityInQueryIndex+(uint)singleton.ElapsedTime);
                float3 dir = math.normalize(-t.Value) + rand.NextFloat3(new float3(-0.05f, -0.05f, 0), new float3(0.05f,0.05f,0));
                PV.Linear = dir;
            }
        }).WithBurst().ScheduleParallel();
    }
}
