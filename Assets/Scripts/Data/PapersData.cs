using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct PapersData : IComponentData
{
    public PaperTypes ThisPaperType;
}

public enum PaperTypes { FirstStation, SecondStation, Boss, Trash };
