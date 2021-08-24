using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SingletonData : IComponentData
{
    public float DeltaTime;
    public float3 PlayerPos;
    public float maxX, maxY;
}
