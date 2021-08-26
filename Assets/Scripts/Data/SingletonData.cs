using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;

[GenerateAuthoringComponent]
public struct SingletonData : IComponentData
{
    [HideInInspector] public float DeltaTime;
    [HideInInspector] public float3 PlayerPos;
    [HideInInspector] public float maxX, maxY;
    [HideInInspector] public int Station1Papers, Station2Papers;
}
