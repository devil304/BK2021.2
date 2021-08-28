using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsScript : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip beepBoop;
    [SerializeField] AudioClip stamp0;
    [SerializeField] AudioClip stamp1;
    [SerializeField] AudioClip papers;

    public void BeepBoop()
    {
        soundSource.PlayOneShot(beepBoop);
    }

    public void Stamp()
    {
        soundSource.PlayOneShot(Random.value < 0.5f ? stamp0 : stamp1);
    }

    public void Papers()
    {
        soundSource.PlayOneShot(papers);
    }
}
