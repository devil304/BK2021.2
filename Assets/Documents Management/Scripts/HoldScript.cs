using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoldScript : MonoBehaviour
{
    [SerializeField] new Collider2D collider;
    [SerializeField] DocumentHolder documents;
    [SerializeField] bool sendToButtons = false;
    [SerializeField] List<GameObject> stampMarkPrefabs;
    [SerializeField] float holdScale = 1.2f;
    [SerializeField] float holdAlpha = 1f;
    new Camera camera;

    static HoldScript heldItem = null;
    static Vector3 holdOffset = Vector3.zero;
    Vector3 baseScale;
    float baseZPos;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("DocumentsMainCamera").GetComponent<Camera>();
        baseScale = transform.localScale;
        baseZPos = transform.position.z;
    }

    void Update()
    {
        Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.position.z;

        collider.enabled = true;
        if (heldItem == null && Input.GetMouseButtonDown(0) && collider.OverlapPoint(pos))
        {
            heldItem = this;

            Vector3 localPos = transform.InverseTransformPoint(pos);
            transform.localScale = baseScale * holdScale;
            pos.z = -3;
            var position = transform.position;
            position.z = -3;
            transform.position = position;
            Vector3 newPos = transform.TransformPoint(localPos);

            transform.localScale = baseScale;
            transform.DOComplete();
            transform.DOScale(baseScale * holdScale, 0.1f);

            holdOffset = transform.position - newPos;

            foreach (var r in GetComponentsInChildren<SpriteRenderer>())
            {
                r.DOComplete();
                r.DOFade(holdAlpha, 0.1f);
            }
        }
        collider.enabled = false;

        if (heldItem == this)
        {
            transform.position = Vector3.MoveTowards(transform.position, holdOffset + pos, 100 * Time.deltaTime);
            if (sendToButtons) FindObjectOfType<ButtonsScript>().Highlight(pos);
            if (!Input.GetMouseButton(0))
            {
                heldItem = null;
                var position = transform.position;
                position.z = baseZPos;
                transform.position = position;

                if (documents != null)
                {
                    FindObjectOfType<SoundsScript>().Stamp();
                    documents.Stamp(collider, stampMarkPrefabs[Random.Range(0, stampMarkPrefabs.Count)]);
                }

                if (sendToButtons)
                {
                    if (!FindObjectOfType<ButtonsScript>().Press(gameObject, pos)) RevertHoldModifiers();
                    else FindObjectOfType<SoundsScript>().Papers();
                }
                else RevertHoldModifiers();
            }
        }
    }

    void RevertHoldModifiers()
    {
        transform.DOScale(baseScale, 0.1f);

        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.DOComplete();
            r.DOFade(1f, 0.1f);
        }
    }
}
