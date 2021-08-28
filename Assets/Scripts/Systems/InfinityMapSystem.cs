using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(StarShipSystem))]
public class InfinityMapSystem : SystemBase
{
    [BurstDiscard]
    public static void Log(string message)
    {
        Debug.Log(message);
    }

    protected override void OnUpdate()
    {
        var singleton = GetSingleton<SingletonData>();
        float3 PlayerPos = singleton.PlayerPos;

        if(singleton.PlayerPos.x > singleton.maxX || singleton.PlayerPos.x <-singleton.maxX || singleton.PlayerPos.y > singleton.maxY || singleton.PlayerPos.y < -singleton.maxY)
        {
            float3 offset = new float3(singleton.PlayerPos.x <-singleton.maxX ? singleton.PlayerPos.x+ singleton.maxX:(singleton.PlayerPos.x > singleton.maxX ? singleton.PlayerPos.x - singleton.maxX: 0),
                singleton.PlayerPos.y < -singleton.maxY ? singleton.PlayerPos.y + singleton.maxY : (singleton.PlayerPos.y > singleton.maxY ? singleton.PlayerPos.y - singleton.maxY : 0), 0);

            Entities.WithNone<StarShipData, LapkiTag>().ForEach((ref Translation t) =>
            {
                if (Vector2.Distance((Vector3)(PlayerPos * -1 + offset), (Vector3)t.Value) < 10)
                {
                    t.Value += new float3(1000, 1000, 0);
                }
                float3 tmpPlayerPos = singleton.PlayerPos * -1 + offset;
                if (Vector2.Distance((Vector3)PlayerPos, (Vector3)t.Value) < 10)
                {
                    t.Value = (t.Value - PlayerPos) + tmpPlayerPos;
                }
                if (Vector2.Distance((Vector3)(PlayerPos * -1 + offset) + ((Vector3)new float3(1000, 1000, 0)), (Vector3)t.Value) < 10)
                {
                    t.Value -= new float3(1000, 1000, 0);
                    t.Value.x *= -1;
                    t.Value.y *= -1;
                    t.Value += offset;
                }
            }).WithBurst().ScheduleParallel();

            Entities.WithAny<StarShipData, LapkiTag>().ForEach((ref Translation t) => {
                if (Vector2.Distance((Vector3)PlayerPos, (Vector3)t.Value) < 10)
                {
                    t.Value.x *= -1;
                    t.Value.y *= -1;
                    t.Value += offset;
                }
            }).WithBurst().ScheduleParallel();

            singleton.PlayerPos *= -1;
            singleton.PlayerPos += offset;

            SetSingleton(singleton);
        }
    }
}
