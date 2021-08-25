using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuoteScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int numberOfAnims = 3;

    IEnumerator Start()
    {
        while (true)
        {
            animator.SetInteger("quote", Random.Range(0, numberOfAnims));
            yield return new WaitForSeconds(8f);
        }
    }
}
