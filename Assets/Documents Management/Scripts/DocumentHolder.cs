using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DocumentHolder : MonoBehaviour
{
    [SerializeField] float maxSpread = 0f;
    [SerializeField] List<GameObject> documentPrefabs;

    [SerializeField] float animTime = 1f;
    [SerializeField] Ease animEase = Ease.OutBack;

    public int currentDocZPos = 0;

    IEnumerator Start()
    {
        while (true)
        {
            // SpawnDocument((Document)Random.Range(0, 4));
            SpawnDocument(Document.One);
            yield return new WaitForSeconds(4f);
            // break;
        }
    }

    public void SpawnDocument(Document document)
    {
        var doc = documentPrefabs[(int)document];
        Vector3 pos = transform.position + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), currentDocZPos);
        var go = Instantiate(doc, pos + new Vector3(0, 10, 0), Quaternion.identity, transform);
        go.transform.DOMoveY(pos.y, animTime).SetEase(animEase);

        currentDocZPos += 1;
    }

    public void Stamp(Collider2D collider, GameObject stampMarkPrefab)
    {
        if (transform.childCount == 0) return;
        var doc = transform.GetChild(0);

        Instantiate(stampMarkPrefab, collider.transform.position, Quaternion.identity, doc);

        foreach (var area in doc.GetComponents<Collider2D>())
        {
            if (!area.enabled) continue;

            float dist = Vector2.Distance(area.bounds.center, collider.bounds.center);

            if (dist < 0.2f && (
                (area is BoxCollider2D && collider is BoxCollider2D) ||
                (area is CircleCollider2D && collider is CircleCollider2D)))
            {
                Destroy(area);
            }
        }
    }

    public void SpaceStationA()
    {
        RemoveUpper();
    }

    public void SpaceStationB()
    {
        RemoveUpper();
    }

    public void Bossu()
    {
        RemoveUpper();
    }

    public void TrashBin()
    {
        RemoveUpper();
    }

    void RemoveUpper()
    {
        if (transform.childCount == 0) return;
        var doc = transform.GetChild(0);
        Debug.Log(doc.GetComponents<Collider2D>().Length);
        doc.parent = null;
        doc.DOMoveY(-10, animTime).SetEase(animEase).onComplete += () =>
        {
            Destroy(doc.gameObject);
        };
    }
}

public enum Document
{
    One,
    Two,
    Three,
    Four
}