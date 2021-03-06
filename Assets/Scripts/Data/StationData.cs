using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[GenerateAuthoringComponent]
public struct StationData : IComponentData
{
    public StationTypes ThisStationType;
    public float TimeToTeleport;
    public float TimeLeftToTeleport;
    [HideInInspector] public Random rand;
    public bool passing;
    public int angriness;
}

public enum StationTypes { FirstStation, SecondStation, Coffie };
