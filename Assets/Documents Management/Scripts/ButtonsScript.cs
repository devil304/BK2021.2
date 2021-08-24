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

    public bool Press(GameObject doc, Vector3 mousePos)
    {
        bossu.transform.localScale = Vector3.one / 2;
        spaceStationA.transform.localScale = Vector3.one / 2;
        spaceStationB.transform.localScale = Vector3.one / 2;
        trash.transform.localScale = Vector3.one / 2;

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
        bossu.transform.localScale = Vector3.one / 2;
        spaceStationA.transform.localScale = Vector3.one / 2;
        spaceStationB.transform.localScale = Vector3.one / 2;
        trash.transform.localScale = Vector3.one / 2;

        if (bossu.OverlapPoint(mousePos)) bossu.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        else if (spaceStationA.OverlapPoint(mousePos)) spaceStationA.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        else if (spaceStationB.OverlapPoint(mousePos)) spaceStationB.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        else if (trash.OverlapPoint(mousePos)) trash.transform.localScale = new Vector3(0.7f, 0.7f, 1);
    }
}
