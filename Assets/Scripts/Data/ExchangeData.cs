using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public class ExchangeData : IComponentData
{
    public ECS_MB_DataExchange EMDE;
}
