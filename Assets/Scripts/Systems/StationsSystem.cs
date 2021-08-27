using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class StationsSystem : SystemBase
{

    protected override void OnStartRunning()
    {
        var singleton = GetSingleton<SingletonData>();
        Entities.ForEach((int entityInQueryIndex,ref Translation t, ref StationData SD) => {
            float dist = Vector2.Distance((Vector3)t.Value, (Vector3)singleton.PlayerPos);
            SD.rand = Random.CreateFromIndex((uint)entityInQueryIndex);
            SD.TimeLeftToTeleport = SD.TimeToTeleport;
            TeleportInFrontOfSS(dist, singleton, ref t,ref SD.rand);
        }).WithBurst().Schedule();
    }

    protected override void OnUpdate()
    {
        var singleton = GetSingleton<SingletonData>();
        Entities.ForEach((ref PhysicsVelocity PV, ref Translation t, ref StationData SD)=> {
            float dist = math.distance(t.Value, singleton.PlayerPos);
            if(!SD.passing && dist <= 10f)
                SD.passing = true;
            else if(SD.passing && dist > 10f)
            {
                SD.passing = false;
                switch (SD.ThisStationType)
                {
                    case StationTypes.FirstStation:
                        if (singleton.Station1Papers > 0)
                            singleton.angriness += singleton.Station1Papers;
                        break;
                    case StationTypes.SecondStation:
                        if (singleton.Station2Papers > 0)
                            singleton.angriness += singleton.Station2Papers;
                        break;
                }
            }
            PV.Angular = new float3(0,1,0);
            SD.TimeLeftToTeleport -= singleton.DeltaTime;
            if (SD.TimeLeftToTeleport <= 0)
            {
                SD.TimeLeftToTeleport = SD.TimeToTeleport;
                TeleportInFrontOfSS(dist, singleton,ref t,ref SD.rand);
            }
            else
                SD.TimeLeftToTeleport -= singleton.DeltaTime;
        }).WithBurst().Schedule();
        Dependency.Complete();

        SetSingleton(singleton);
    }

    static void TeleportInFrontOfSS(float dist, SingletonData singleton,ref Translation t,ref Random r)
    {
        if (dist>11 && singleton.PlayerPos.y < (singleton.maxY - 10))
        {
            r = new Random(r.NextUInt(1,uint.MaxValue-10000)+(uint)(singleton.DeltaTime*1000f));
            float3 NewPos = new float3(r.NextFloat(singleton.PlayerPos.x - 12f, singleton.PlayerPos.x + 12f),
                r.NextFloat(singleton.PlayerPos.y + 10, singleton.maxY), 0);
            t.Value = NewPos;
        }
    }
}