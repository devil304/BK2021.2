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

    /*[BurstDiscard]
    public static void Log(string message)
    {
        Debug.Log(message); // actually have custom logging stuff here but simplified it
    }*/

    protected override void OnCreate()
    {
        BPW = World.GetOrCreateSystem<BuildPhysicsWorld>();
        SPW = World.GetOrCreateSystem<StepPhysicsWorld>();
        BIECommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        Sd = new SingletonData();
    }
    Entity SS;
    protected override void OnStartRunning()
    {
        Entities.WithAll<StarShipData>().ForEach((Entity e)=> {
            SS = e;
        }).WithoutBurst().Run();
    }

    public SingletonData Sd;
    protected override void OnUpdate()
    {
        var ss = SS;
        var commandBuffer = BIECommandBuffer.CreateCommandBuffer();
        Dependency = new EntityCollision
        {
            AsteroidsGroup = GetComponentDataFromEntity<AsteroidTag>(true),
            PapersGroup = GetComponentDataFromEntity<PapersData>(),
            StarShip = GetComponentDataFromEntity<StarShipData>(),
            StationsGroup = GetComponentDataFromEntity<StationData>(),
            LapkiGroup = GetComponentDataFromEntity<LapkiTag>(),
            ECB = commandBuffer,
            SD = ss
        }.Schedule(SPW.Simulation, ref BPW.PhysicsWorld, Dependency);
        Dependency.Complete();
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
        public Entity SD;
        public EntityCommandBuffer ECB;

        public void Execute(CollisionEvent collisionEvent)
        {
            if((StarShip.HasComponent(collisionEvent.EntityA)|| StarShip.HasComponent(collisionEvent.EntityB)))
            {
                if ((!PapersGroup.HasComponent(collisionEvent.EntityA) && !PapersGroup.HasComponent(collisionEvent.EntityB)))
                {
                    var tmp = StarShip[SD];
                    tmp.angrinessTmp += 1;
                    StarShip[SD] = tmp;
                }
            }
            else if ((LapkiGroup.HasComponent(collisionEvent.EntityA) || LapkiGroup.HasComponent(collisionEvent.EntityB)))
            {
                var tmp = StarShip[SD];
                if (PapersGroup.HasComponent(collisionEvent.EntityA))
                {
                    switch (PapersGroup[collisionEvent.EntityA].ThisPaperType)
                    {
                        case PaperTypes.FirstStation:
                            tmp.Station1Papers += 1;
                            break;
                        case PaperTypes.SecondStation:
                            tmp.Station2Papers += 1;
                            break;
                        case PaperTypes.Trash:
                            tmp.TrashPapers += 1;
                            break;
                        case PaperTypes.Boss:
                            tmp.BossPapers += 1;
                            break;
                    }
                    ECB.DestroyEntity(collisionEvent.EntityA);
                }
                else if (PapersGroup.HasComponent(collisionEvent.EntityB))
                {
                    switch (PapersGroup[collisionEvent.EntityB].ThisPaperType)
                    {
                        case PaperTypes.FirstStation:
                            tmp.Station1Papers += 1;
                            break;
                        case PaperTypes.SecondStation:
                            tmp.Station2Papers += 1;
                            break;
                        case PaperTypes.Trash:
                            tmp.TrashPapers += 1;
                            break;
                        case PaperTypes.Boss:
                            tmp.BossPapers += 1;
                            break;
                    }
                    ECB.DestroyEntity(collisionEvent.EntityB);
                }
                StarShip[SD] = tmp;
            }
        }
    }
}
