using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DocumentHolder : MonoBehaviour
{
    [SerializeField] float maxSpread = 0f;
    [SerializeField] List<GameObject> documentPrefabs;

    public int currentDocZPos = 0;

    public TextMeshProUGUI stationAText;
    public SpriteRenderer stationASpriteRend;
    public Sprite stationASprite;
    public Sprite stationAAltSprite;
    int stationADocuments = 0;
    public int StationADocuments
    {
        get => stationADocuments;
        set
        {
            stationADocuments = value;
            stationAText.text = stationADocuments.ToString();
            stationASpriteRend.sprite = stationADocuments == 0 ? stationAAltSprite : stationASprite;
        }
    }

    public TextMeshProUGUI stationBText;
    public SpriteRenderer stationBSpriteRend;
    public Sprite stationBSprite;
    public Sprite stationBAltSprite;
    int stationBDocuments = 0;
    public int StationBDocuments
    {
        get => stationBDocuments;
        set
        {
            stationBDocuments = value;
            stationBText.text = stationBDocuments.ToString();
            stationBSpriteRend.sprite = stationBDocuments == 0 ? stationBAltSprite : stationBSprite;
        }
    }

    [SerializeField] float bossLowerTimer = 10f;
    float currectBossLowerTimer = 0f;
    public Transform bossIndicator;
    int bossDocuments = 0;
    public int BossDocuments
    {
        get => bossDocuments;
        set
        {
            if (value > 5) QuoteScript.FullFolder();
            bossDocuments = Mathf.Clamp(value, 1, 5);
            for (int i = 0; i <= 5; i++)
            {
                bossIndicator.GetChild(i).gameObject.SetActive(i <= bossDocuments);
            }
        }
    }

    public Vector3 GetRandomDocumentPosition() =>
            transform.position + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), currentDocZPos);

    IEnumerator Start()
    {
        StationADocuments = 0;
        StationBDocuments = 0;
        BossDocuments = 3;
        currectBossLowerTimer = bossLowerTimer;
        while (true)
        {
            SpawnDocument(documentPrefabs[Random.Range(0, documentPrefabs.Count)]);
            yield return new WaitForSeconds(4f);
        }
    }

    void Update()
    {
        if (transform.childCount != 0) transform.GetChild(0).GetComponent<HoldScript>().enabled = true;

        if (currectBossLowerTimer <= 0f)
        {
            currectBossLowerTimer = bossLowerTimer;
            BossDocuments -= 1;
        }
        else currectBossLowerTimer -= Time.deltaTime;

        if (bossDocuments == 1 || bossDocuments == 5)
        {
            Frustration.Value += 0.025f * Time.deltaTime;
        }
    }

    public void SpawnDocument(GameObject document)
    {
        Vector3 pos = GetRandomDocumentPosition();
        var go = Instantiate(document, pos + new Vector3(0, 10, 0), Quaternion.identity, transform);
        go.transform.DOMoveY(pos.y, 0.7f).SetEase(Ease.OutExpo);

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

            if (dist < 0.3f && (
                (area is BoxCollider2D && collider is BoxCollider2D) ||
                (area is CircleCollider2D && collider is CircleCollider2D) ||
                (area is PolygonCollider2D && collider is PolygonCollider2D)))
            {
                Destroy(area);
            }
        }
    }

    public void SpaceStationA()
    {
        if (transform.childCount == 0) return;
        var doc = transform.GetChild(0);
        if (doc.CompareTag("DocumentA"))
        {
            if (doc.GetComponents<Collider2D>().Length == 1)
            {
                RemoveUpper(() => StationADocuments++);
            }
            else
            {
                RemoveUpper(() =>
                {
                    Frustration.Value += 0.1f;
                    QuoteScript.IncorrectlyStamped();
                });
            }
        }
        else
        {
            RemoveUpper(() =>
            {
                Frustration.Value += 0.1f;
                QuoteScript.IncorrectFolder();
            });
        }
    }

    public void SpaceStationB()
    {
        if (transform.childCount == 0) return;
        var doc = transform.GetChild(0);
        if (doc.CompareTag("DocumentB"))
        {
            if (doc.GetComponents<Collider2D>().Length == 1)
            {
                RemoveUpper(() => StationBDocuments++);
            }
            else
            {
                RemoveUpper(() =>
                {
                    Frustration.Value += 0.1f;
                    QuoteScript.IncorrectlyStamped();
                });
            }
        }
        else
        {
            RemoveUpper(() =>
            {
                Frustration.Value += 0.1f;
                QuoteScript.IncorrectFolder();
            });
        }
    }

    public void Bossu()
    {
        if (transform.childCount == 0) return;
        var doc = transform.GetChild(0);
        if (doc.CompareTag("DocumentBossu"))
        {
            if (doc.GetComponents<Collider2D>().Length == 1)
            {
                RemoveUpper(() => BossDocuments++);
            }
            else
            {
                RemoveUpper(() =>
                {
                    Frustration.Value += 0.1f;
                    QuoteScript.IncorrectlyStamped();
                });
            }
        }
        else
        {
            RemoveUpper(() =>
            {
                Frustration.Value += 0.1f;
                QuoteScript.IncorrectFolder();
            });
        }
    }

    public void Trash()
    {
        if (transform.childCount == 0) return;
        RemoveUpper(() => { });
    }

    void RemoveUpper(System.Action action)
    {
        var doc = transform.GetChild(0);

        var position = doc.transform.position;
        position.z = -3;
        doc.transform.position = position;

        doc.parent = null;
        doc.DOMoveX(10, 0.4f).SetEase(Ease.InQuart).onComplete += () =>
        {
            action();
            Destroy(doc.gameObject);
        };
    }
}