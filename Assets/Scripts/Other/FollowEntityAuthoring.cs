using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class FollowEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] GameObject follower;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        FollowEntity followEntity = follower.GetComponent<FollowEntity>();
        followEntity.EManager = dstManager;
        followEntity.entityToFollow = entity;
    }
}
