using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct StarShipData : IComponentData
{
    public float MaxSpeedUp, MinSpeedUp,
        speedSides,
        acceleration;
    public float maxX, maxY;
    public float timer;
    public float3 pointOne, pointTwo;
    public Entity paper1, paper2, trash, boss;
    public int angriness;

    public int Station1Papers, Station2Papers, TrashPapers, BossPapers;
    public float angrinessTmp;
}
