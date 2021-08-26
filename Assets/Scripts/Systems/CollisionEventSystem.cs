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
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollisionEventSystem : SystemBase
{
    BuildPhysicsWorld BPW;
    StepPhysicsWorld SPW;

    protected override void OnCreate()
    {
        BPW = World.GetOrCreateSystem<BuildPhysicsWorld>();
        SPW = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        Dependency = new EntityCollision
        {
            AsteroidsGroup = GetComponentDataFromEntity<AsteroidTag>(true),
            PapersGroup = GetComponentDataFromEntity<PapersData>(),
            StarShip = GetComponentDataFromEntity<StarShipData>(),
            StationsGroup = GetComponentDataFromEntity<StationData>()
        }.Schedule(SPW.Simulation, ref BPW.PhysicsWorld, Dependency);
    }

    [BurstCompile]
    struct EntityCollision : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<AsteroidTag> AsteroidsGroup;
        public ComponentDataFromEntity<PapersData> PapersGroup;
        public ComponentDataFromEntity<StarShipData> StarShip;
        public ComponentDataFromEntity<StationData> StationsGroup;
        public void Execute(CollisionEvent collisionEvent)
        {

        }
    }
}
