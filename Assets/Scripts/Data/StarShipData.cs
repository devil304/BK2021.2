using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct StarShipData : IComponentData
{
    public float MaxSpeedUp, MinSpeedUp,
        speedSides,
        acceleration;
    public float maxX, maxY;
}
