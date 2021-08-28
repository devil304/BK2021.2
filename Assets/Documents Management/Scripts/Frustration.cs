using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Frustration : MonoBehaviour
{
    [SerializeField] GameObject mask;
    [SerializeField] CanvasGroup endGroup;

    float value = 0f;

    /// <summary> From 0 to 1. </summary>
    public static float Value
    {
        get => FindObjectOfType<Frustration>().value;
        set { FindObjectOfType<Frustration>().value = value; }
    }

    void Start()
    {
        value = 0f;
        var pos = mask.transform.position;
        pos.y = Mathf.Lerp(-10, 0, value);
        mask.transform.position = pos;
    }

    void Update()
    {
        var pos = mask.transform.position;
        pos.y = Mathf.MoveTowards(pos.y, Mathf.Lerp(-10, 0, value), 3 * Time.deltaTime);
        mask.transform.position = pos;

        if (Value >= 1f)
        {
            enabled = false;
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        endGroup.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(2f);
        Unity.Entities.World.DisposeAllWorlds();
    }
}
