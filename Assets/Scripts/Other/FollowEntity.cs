using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;

    public EntityManager EManager;

    [SerializeField] Vector3 offset;

    void LateUpdate()
    {
        try
        {
            if (EManager.Exists(entityToFollow))
            {
                Translation entityTrans = EManager.GetComponentData<Translation>(entityToFollow);
                Vector3 EPos = (Vector3)entityTrans.Value-offset;
                EPos.z = transform.position.z;
                transform.position = EPos;
            }
        }
        catch { }
    }
}
