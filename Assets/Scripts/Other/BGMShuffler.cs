using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMShuffler : MonoBehaviour
{
    [SerializeField] AudioClip[] BGMs;
    AudioSource MyAS;
    int LastClip=-1;

    // Start is called before the first frame update
    void Start()
    {
        MyAS = GetComponent<AudioSource>();
        LastClip = Random.Range(0, BGMs.Length);
        MyAS.clip = BGMs[LastClip];
        MyAS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        while (true)
        {
            int tmp = Random.Range(0, BGMs.Length);
            if(tmp != LastClip)
            {
                LastClip = tmp;
                break;
            }
        }
        if (!MyAS.isPlaying)
        {
            MyAS.clip = BGMs[LastClip];
            MyAS.Play();
        }
    }
}
