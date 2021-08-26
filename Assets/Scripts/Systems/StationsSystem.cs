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
            TeleportInFrontOfSS(dist, singleton, ref t, SD.rand);
        }).WithBurst().Schedule();
    }

    protected override void OnUpdate()
    {
        var singleton = GetSingleton<SingletonData>();
        Entities.ForEach((ref PhysicsVelocity PV, ref Translation t, ref StationData SD)=> {
            float dist = Vector2.Distance((Vector3)t.Value, (Vector3)singleton.PlayerPos);
            if(!SD.passing && dist <= 4.5f)
                SD.passing = true;
            else if(SD.passing && dist > 4.5f)
            {
                SD.passing = false;
                if ((SD.ThisStationType == StationTypes.FirstStation && singleton.Station1Papers != 0) ||
                (SD.ThisStationType == StationTypes.SecondStation && singleton.Station2Papers != 0))
                {
                    //Passed station with papers
                }
            }
            PV.Angular = new float3(0,1,0);
            SD.TimeLeftToTeleport -= singleton.DeltaTime;
            if (SD.TimeLeftToTeleport <= 0)
            {
                SD.TimeLeftToTeleport = SD.TimeToTeleport;
                TeleportInFrontOfSS(dist, singleton,ref t, SD.rand);
            }
            else
                SD.TimeLeftToTeleport -= singleton.DeltaTime;
        }).WithBurst().Schedule();
    }

    static void TeleportInFrontOfSS(float dist, SingletonData singleton,ref Translation t, Random r)
    {
        if (dist>8 && singleton.PlayerPos.y < (singleton.maxY - 8))
        {
            float3 NewPos = new float3(r.NextFloat(singleton.PlayerPos.x - 10f, singleton.PlayerPos.x + 10f),
                r.NextFloat(singleton.PlayerPos.y + 8, singleton.maxY), 0);
            t.Value = NewPos;
        }
    }
}