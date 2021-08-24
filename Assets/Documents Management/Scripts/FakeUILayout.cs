using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeUILayout : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float xPosition;

    [ContextMenu("Set")]
    void Start()
    {
        Camera camera = GameObject.FindGameObjectWithTag("DocumentsMainCamera").GetComponent<Camera>();
        Vector3 left_up_corner = new Vector3(Mathf.Lerp(Screen.width * camera.rect.xMin, Screen.width * camera.rect.xMax, xPosition), 0, 0);
        float x = camera.ScreenToWorldPoint(left_up_corner).x;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        Destroy(this);
    }
}
