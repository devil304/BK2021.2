using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button2D : MonoBehaviour
{
    [SerializeField] new Collider2D collider;
    [SerializeField] UnityEvent click;
    new Camera camera;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("DocumentsMainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && collider.OverlapPoint(pos)) click.Invoke();
    }
}
