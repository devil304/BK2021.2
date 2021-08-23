using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitSpriteToCamera : MonoBehaviour
{
    void OnEnable()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No sprite renderer", gameObject);
            return;
        }

        transform.localScale = new Vector3(1, 1, 1);
        var size = spriteRenderer.bounds.size;

        Camera camera = GameObject.FindGameObjectWithTag("DocumentsMainCamera").GetComponent<Camera>();
        float screenHeight = camera.orthographicSize * 2;
        float screenWidth = screenHeight / (Screen.height * camera.rect.height) * (Screen.width * camera.rect.width);

        transform.localScale = new Vector3(screenWidth / size.x, screenHeight / size.y, 1);
    }
}
