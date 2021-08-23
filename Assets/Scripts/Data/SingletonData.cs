using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct SingletonData : IComponentData
{
    [HideInInspector] public float DeltaTime;
}
