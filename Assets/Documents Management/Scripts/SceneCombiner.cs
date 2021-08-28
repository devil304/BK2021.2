using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCombiner : MonoBehaviour
{
    [SerializeField] string spaceScene;
    [SerializeField] DocumentHolder documents;
    [SerializeField] List<GameObject> stationAPrefabs;
    [SerializeField] List<GameObject> stationBPrefabs;
    [SerializeField] List<GameObject> bossPrefabs;
    [SerializeField] List<GameObject> trashPrefabs;

    public int documentsAToUpdate = 0;
    public int documentsBToUpdate = 0;

    void Start()
    {
        SceneManager.LoadScene(spaceScene, LoadSceneMode.Additive);
    }

    void Update()
    {
        var dataExchange = FindObjectOfType<ECS_MB_DataExchange>();
        if (dataExchange == null) return;
        var sd = dataExchange.SD;

        for (int i = 0; i < sd.Station1Papers; i++) documents.SpawnDocument(stationAPrefabs[Random.Range(0, stationAPrefabs.Count)]);
        for (int i = 0; i < sd.Station2Papers; i++) documents.SpawnDocument(stationBPrefabs[Random.Range(0, stationBPrefabs.Count)]);
        for (int i = 0; i < sd.BossPapers; i++) documents.SpawnDocument(bossPrefabs[Random.Range(0, bossPrefabs.Count)]);
        for (int i = 0; i < sd.TrashPapers; i++) documents.SpawnDocument(trashPrefabs[Random.Range(0, trashPrefabs.Count)]);

        Frustration.Value += sd.angriness / 100.0f;

        sd.Station1Papers = 0;
        sd.Station2Papers = 0;
        sd.BossPapers = 0;
        sd.TrashPapers = 0;
        sd.angriness = 0;

        sd.Station1PapersR += documentsAToUpdate;
        sd.Station2PapersR += documentsBToUpdate;

        documentsAToUpdate = 0;
        documentsBToUpdate = 0;

        documents.stationADocumentsCollected = sd.Papers1Collected;
        documents.stationBDocumentsCollected = sd.Papers2Collected;

        dataExchange.SD = sd;

        documents.UpdatePapersCollected();
    }
}
