using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;

    public EntityManager EManager;

    void LateUpdate()
    {
        try
        {
            if (EManager.Exists(entityToFollow))
            {
                Translation entityTrans = EManager.GetComponentData<Translation>(entityToFollow);
                Vector3 EPos = entityTrans.Value;
                EPos.z = transform.position.z;
                transform.position = EPos;
            }
        }
        catch { }
    }
}
