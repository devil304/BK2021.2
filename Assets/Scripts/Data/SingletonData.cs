using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

[GenerateAuthoringComponent]
public struct SingletonData : IComponentData
{
    public float DeltaTime, ElapsedTime;
    public float3 PlayerPos;
    public float maxX, maxY;
    public int Station1Papers, Station2Papers;
}
