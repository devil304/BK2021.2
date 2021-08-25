using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuoteScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int numberOfAnims = 3;

    public static void IncorrectFolder()
    {
        var self = FindObjectOfType<QuoteScript>();
        self.StopAllCoroutines();
        self.animator.SetInteger("quote", -1);
        self.StartCoroutine("Start");
    }

    public static void IncorrectlyStamped()
    {
        var self = FindObjectOfType<QuoteScript>();
        self.StopAllCoroutines();
        self.animator.SetInteger("quote", -2);
        self.StartCoroutine("Start");
    }

    public static void FullFolder()
    {
        var self = FindObjectOfType<QuoteScript>();
        self.StopAllCoroutines();
        self.animator.SetInteger("quote", -3);
        self.StartCoroutine("Start");
    }

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            animator.SetInteger("quote", Random.Range(0, numberOfAnims));
        }
    }
}
