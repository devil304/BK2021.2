using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampScript : MonoBehaviour
{
    [SerializeField] new Collider2D collider;
    [SerializeField] DocumentHolder documents;
    [SerializeField] GameObject stampMarkPrefab;
    new Camera camera;

    static StampScript holdingStamp = null;
    static Vector3 holdOffset = Vector3.zero;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("DocumentsMainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.position.z;

        if (holdingStamp == null && Input.GetMouseButtonDown(0) && collider.OverlapPoint(pos))
        {
            holdingStamp = this;
            holdOffset = transform.position - pos;
        }

        if (holdingStamp == this)
        {
            if (Input.GetMouseButtonUp(0))
            {
                holdingStamp = null;
                documents.Stamp(collider, stampMarkPrefab);
            }

            transform.position = holdOffset + pos;
        }
    }
}
