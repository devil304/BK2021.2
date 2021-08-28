using Unity.Entities;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(StationsSystem))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollisionEventSystem : SystemBase
{
    BuildPhysicsWorld BPW;
    StepPhysicsWorld SPW;

    BeginInitializationEntityCommandBufferSystem BIECommandBuffer;

    [BurstDiscard]
    public static void Log(string message)
    {
        Debug.Log(message); // actually have custom logging stuff here but simplified it
    }

    protected override void OnCreate()
    {
        BPW = World.GetOrCreateSystem<BuildPhysicsWorld>();
        SPW = World.GetOrCreateSystem<StepPhysicsWorld>();
        BIECommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        Sd = new SingletonData();
    }
    public static SingletonData Sd;
    protected override void OnUpdate()
    {
        var commandBuffer = BIECommandBuffer.CreateCommandBuffer();
        var singleton = GetSingleton<SingletonData>();
        Sd.DeltaTime = singleton.DeltaTime;
        Dependency = new EntityCollision
        {
            AsteroidsGroup = GetComponentDataFromEntity<AsteroidTag>(true),
            PapersGroup = GetComponentDataFromEntity<PapersData>(),
            StarShip = GetComponentDataFromEntity<StarShipData>(),
            StationsGroup = GetComponentDataFromEntity<StationData>(),
            LapkiGroup = GetComponentDataFromEntity<LapkiTag>(),
            ECB = commandBuffer
        }.Schedule(SPW.Simulation, ref BPW.PhysicsWorld, Dependency);
        Dependency.Complete();
        if (Sd.angrinessTmp != 0 || Sd.Station1Papers != 0 || Sd.Station2Papers != 0 || Sd.BossPapers != 0 || Sd.TrashPapers != 0)
        {
            if (Sd.angrinessTmp > 1)
            {
                singleton.angriness += (int)Sd.angrinessTmp;
                Sd.angrinessTmp %= 1f;
            }
            singleton.Station1Papers += Sd.Station1Papers;
            singleton.Station2Papers += Sd.Station2Papers;
            singleton.BossPapers += Sd.BossPapers;
            singleton.TrashPapers += Sd.TrashPapers;
            Log("Tak");
            Sd.angriness = 0;
            Sd.Station1Papers = 0;
            Sd.Station2Papers = 0;
            Sd.BossPapers = 0;
            Sd.TrashPapers = 0;
        }
        SetSingleton(singleton);
        BIECommandBuffer.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    struct EntityCollision : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<AsteroidTag> AsteroidsGroup;
        public ComponentDataFromEntity<PapersData> PapersGroup;
        public ComponentDataFromEntity<StarShipData> StarShip;
        public ComponentDataFromEntity<StationData> StationsGroup;
        public ComponentDataFromEntity<LapkiTag> LapkiGroup;
        public SingletonData SD;
        public EntityCommandBuffer ECB;

        public void Execute(CollisionEvent collisionEvent)
        {
            if((StarShip.HasComponent(collisionEvent.EntityA)|| StarShip.HasComponent(collisionEvent.EntityB)))
            {
                if ((!PapersGroup.HasComponent(collisionEvent.EntityA) && !PapersGroup.HasComponent(collisionEvent.EntityB)))
                    Sd.angrinessTmp += Sd.DeltaTime*5;
            }
            else if ((LapkiGroup.HasComponent(collisionEvent.EntityA) || LapkiGroup.HasComponent(collisionEvent.EntityB)))
            {
                if (PapersGroup.HasComponent(collisionEvent.EntityA))
                {
                    switch (PapersGroup[collisionEvent.EntityA].ThisPaperType)
                    {
                        case PaperTypes.FirstStation:
                            Sd.Station1Papers += 1;
                            break;
                        case PaperTypes.SecondStation:
                            Sd.Station2Papers += 1;
                            break;
                        case PaperTypes.Trash:
                            Sd.TrashPapers += 1;
                            break;
                        case PaperTypes.Boss:
                            Sd.BossPapers += 1;
                            break;
                    }
                    ECB.DestroyEntity(collisionEvent.EntityA);
                }
                else if (PapersGroup.HasComponent(collisionEvent.EntityB))
                {
                    switch (PapersGroup[collisionEvent.EntityB].ThisPaperType)
                    {
                        case PaperTypes.FirstStation:
                            Sd.Station1Papers += 1;
                            break;
                        case PaperTypes.SecondStation:
                            Sd.Station2Papers += 1;
                            break;
                        case PaperTypes.Trash:
                            Sd.TrashPapers += 1;
                            break;
                        case PaperTypes.Boss:
                            Sd.BossPapers += 1;
                            break;
                    }
                    ECB.DestroyEntity(collisionEvent.EntityB);
                }
            }
        }
    }
}
