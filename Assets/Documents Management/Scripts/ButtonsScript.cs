using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonsScript : MonoBehaviour
{
    [SerializeField] DocumentHolder documents;

    [SerializeField] Collider2D bossu;
    [SerializeField] Collider2D spaceStationA;
    [SerializeField] Collider2D spaceStationB;
    [SerializeField] Collider2D trash;

    Vector3 baseScale;

    void Start()
    {
        baseScale = trash.transform.localScale;
    }

    public bool Press(GameObject doc, Vector3 mousePos)
    {
        bossu.transform.localScale = baseScale;
        spaceStationA.transform.localScale = baseScale;
        spaceStationB.transform.localScale = baseScale;
        trash.transform.localScale = baseScale;

        if (bossu.OverlapPoint(mousePos)) documents.Bossu();
        else if (spaceStationA.OverlapPoint(mousePos)) documents.SpaceStationA();
        else if (spaceStationB.OverlapPoint(mousePos)) documents.SpaceStationB();
        else if (trash.OverlapPoint(mousePos)) documents.Trash();
        else
        {
            var pos = documents.GetRandomDocumentPosition();
            pos.z = doc.transform.position.z;
            doc.transform.DOMove(pos, 0.15f).SetEase(Ease.OutQuad);

            return false;
        }

        return true;
    }

    public void Highlight(Vector3 mousePos)
    {
        bossu.transform.localScale = baseScale;
        spaceStationA.transform.localScale = baseScale;
        spaceStationB.transform.localScale = baseScale;
        trash.transform.localScale = baseScale;

        if (bossu.OverlapPoint(mousePos)) bossu.transform.localScale = baseScale * 1.2f;
        else if (spaceStationA.OverlapPoint(mousePos)) spaceStationA.transform.localScale = baseScale * 1.2f;
        else if (spaceStationB.OverlapPoint(mousePos)) spaceStationB.transform.localScale = baseScale * 1.2f;
        else if (trash.OverlapPoint(mousePos)) trash.transform.localScale = baseScale * 1.2f;
    }
}
