using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{

    public AudioClip pickCorrect;

    public AudioClip pickWrong;

    private AudioSource audioSource;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void CollectGarbage(bool isPickedRight)
    {
        if(isPickedRight) audioSource.PlayOneShot(pickCorrect);
        else audioSource.PlayOneShot(pickWrong);
        
        animator.SetBool("PickedStatus", isPickedRight);
        animator.SetTrigger("Collect");
    }

    public void OnSmashed()
    {
        animator.SetTrigger("Smashed");
    }

}
