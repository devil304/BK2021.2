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
    BeginInitializationEntityCommandBufferSystem BIECommandBuffer;

    protected override void OnCreate()
    {
        BIECommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnStartRunning()
    {
        var commandBuffer = BIECommandBuffer.CreateCommandBuffer();
        var singleton = GetSingleton<SingletonData>();
        Entities.ForEach((Entity e, ref PhysicsVelocity vel, in StarShipData SSD) => {
            vel.Linear = new float3(0,(SSD.MinSpeedUp+SSD.MaxSpeedUp)/2f, 0);
            singleton.maxX = SSD.maxX;
            singleton.maxY = SSD.maxY;
            DynamicBuffer<PapersPRefabBufferData> dynamicBuffer = commandBuffer.AddBuffer<PapersPRefabBufferData>(e);
            dynamicBuffer.Add(new PapersPRefabBufferData { Value = SSD.paper1 });
            dynamicBuffer.Add(new PapersPRefabBufferData { Value = SSD.paper2 });
            dynamicBuffer.Add(new PapersPRefabBufferData { Value = SSD.trash });
            dynamicBuffer.Add(new PapersPRefabBufferData { Value = SSD.boss });
        }).WithBurst().Run();
        singleton.closestStation = new float3(float.MaxValue, float.MaxValue, 0);
        SetSingleton(singleton);

        BIECommandBuffer.AddJobHandleForProducer(Dependency);
    }

    protected override void OnUpdate()
    {
        var commandBuffer = BIECommandBuffer.CreateCommandBuffer();
        var singleton = GetSingleton<SingletonData>();

        Entities.WithAll<StationData>().ForEach((ref StationData SD, in Translation st) => {
            if (math.distance(singleton.PlayerPos, st.Value) < math.distance(singleton.PlayerPos, singleton.closestStation))
            {
                singleton.closestStation = st.Value;
                singleton.closestStationType = SD.ThisStationType;
            }
            singleton.angriness += SD.angriness;
            SD.angriness = 0;
        }).WithBurst().Run();
        //Dependency.Complete();

        singleton.DeltaTime = (float)Time.ElapsedTime-singleton.ElapsedTime;
        singleton.ElapsedTime = (float)Time.ElapsedTime;
        Random rand = new Random((uint)singleton.ElapsedTime+1);
        Entities.ForEach((Entity e, ref Rotation r, ref PhysicsVelocity vel, ref StarShipData SSD, in Translation t) => {
            vel.Angular = float3.zero;
            r.Value = quaternion.identity;

            SSD.timer += singleton.DeltaTime;
            if (SSD.timer > 7.5f)
            {
                SSD.timer = 0;
                DynamicBuffer<PapersPRefabBufferData> dynamicBuffer = GetBuffer<PapersPRefabBufferData>(e);
                if (rand.NextUInt(0, 10) < 5)
                {
                    uint i = rand.NextUInt(3, 10);
                    for (uint x = 0; x <= i; x++)
                    {
                        var BInstance = commandBuffer.Instantiate(dynamicBuffer[rand.NextInt(0, 4)].Value);
                        commandBuffer.SetComponent(BInstance, new Translation { Value = t.Value + SSD.pointOne });
                        commandBuffer.SetComponent(BInstance, new Rotation { Value = rand.NextQuaternionRotation() });
                        commandBuffer.SetComponent(BInstance, new PhysicsVelocity
                        {
                            Linear = rand.NextFloat3(new float3(0.01f, 0.05f, 0),
                            new float3(0.05f, 0.01f, 0))
                        });
                    }
                    i = rand.NextUInt(3, 10);
                    for (uint x = 0; x <= i; x++)
                    {
                        var BInstance = commandBuffer.Instantiate(dynamicBuffer[rand.NextInt(0, 4)].Value);
                        commandBuffer.SetComponent(BInstance, new Translation { Value = t.Value + SSD.pointTwo });
                        commandBuffer.SetComponent(BInstance, new Rotation { Value = rand.NextQuaternionRotation() });
                        commandBuffer.SetComponent(BInstance, new PhysicsVelocity
                        {
                            Linear = rand.NextFloat3(new float3(-0.01f, 0.05f, 0),
                            new float3(-0.05f, 0.01f, 0))
                        });
                    }
                }
            }

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

            if (!SSD.clicked && Input.GetKeyDown(KeyCode.E) && math.distance(singleton.PlayerPos, singleton.closestStation)<4)
            {
                SSD.clicked = true;
                switch (singleton.closestStationType)
                {
                    case StationTypes.FirstStation:
                        if (singleton.Station1Papers > 0)
                            singleton.Station1Papers = 0;
                        else
                        {
                            singleton.angriness += 10;
                        }                            
                        break;
                    case StationTypes.SecondStation:
                        if (singleton.Station2Papers > 0)
                            singleton.Station2Papers = 0;
                        else
                        {
                            singleton.angriness += 10;
                        }
                        break;
                    case StationTypes.Coffie:
                        singleton.angriness -= 10;
                        break;
                }
            }
            if (math.distance(singleton.PlayerPos, singleton.closestStation) >= 4)
                singleton.closestStation = new float3(float.MaxValue, float.MaxValue, 0);
        }).WithBurst().Run();

        SetSingleton(singleton);

        Entities.ForEach((ExchangeData ED) => {
            if(ED.EMDE.SD.angriness == 0)
            {
                singleton.angriness -= ED.angriness;
                singleton.Station1Papers -= ED.Papers1;
                singleton.Station2Papers -= ED.Papers2;
                singleton.TrashPapers -= ED.PapersT;
                singleton.BossPapers -= ED.PapersB;
            }
            ED.EMDE.SD = singleton;
            ED.angriness = singleton.angriness;
            ED.angriness = singleton.angriness;
            ED.Papers1 = singleton.Station1Papers;
            ED.Papers2 = singleton.Station2Papers;
            ED.PapersT = singleton.TrashPapers;
            ED.PapersB = singleton.BossPapers;
        }).WithoutBurst().Run();

        SetSingleton(singleton);

        BIECommandBuffer.AddJobHandleForProducer(Dependency);
    }
}
