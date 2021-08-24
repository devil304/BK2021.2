using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct StationData : IComponentData
{
    public StationTypes ThisStationType;
}

public enum StationTypes { FirstStation, SecondStation, Coffie };
