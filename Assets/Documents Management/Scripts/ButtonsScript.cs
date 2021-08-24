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

    public void Press(GameObject doc, Vector3 mousePos)
    {
        if (bossu.OverlapPoint(mousePos)) documents.Bossu();
        else if (spaceStationA.OverlapPoint(mousePos)) documents.SpaceStationA();
        else if (spaceStationB.OverlapPoint(mousePos)) documents.SpaceStationB();
        else if (trash.OverlapPoint(mousePos)) documents.Trash();
        else
        {
            var pos = documents.GetRandomDocumentPosition();
            pos.z = doc.transform.position.z;
            doc.transform.DOMove(pos, 0.15f).SetEase(Ease.OutQuad);
        }

        bossu.transform.localScale = Vector3.one;
        spaceStationA.transform.localScale = Vector3.one;
        spaceStationB.transform.localScale = Vector3.one;
        trash.transform.localScale = Vector3.one;
    }

    public void Highlight(Vector3 mousePos)
    {
        bossu.transform.localScale = Vector3.one;
        spaceStationA.transform.localScale = Vector3.one;
        spaceStationB.transform.localScale = Vector3.one;
        trash.transform.localScale = Vector3.one;

        if (bossu.OverlapPoint(mousePos)) bossu.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        else if (spaceStationA.OverlapPoint(mousePos)) spaceStationA.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        else if (spaceStationB.OverlapPoint(mousePos)) spaceStationB.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        else if (trash.OverlapPoint(mousePos)) trash.transform.localScale = new Vector3(1.3f, 1.3f, 1);
    }
}
