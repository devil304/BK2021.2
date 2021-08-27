using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[InternalBufferCapacity(4)]
[GenerateAuthoringComponent]
public struct PapersPRefabBufferData : IBufferElementData
{
    public Entity Value;
}
