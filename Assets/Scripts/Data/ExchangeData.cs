using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public class ExchangeData : IComponentData
{
    public ECS_MB_DataExchange EMDE;
    public int angriness;
    public int Papers1, Papers2, PapersT, PapersB;
}
