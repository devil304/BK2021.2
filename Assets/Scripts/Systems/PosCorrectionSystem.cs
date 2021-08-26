using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class PosCorrectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity PV, ref Translation t) =>
        {
            PV.Linear.z = 0;
            t.Value.z = 0;
        }).WithBurst().ScheduleParallel();
    }
}
