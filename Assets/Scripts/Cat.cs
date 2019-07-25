using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{

    public AudioClip pickCorrect;

    public AudioClip pickWrong;

    public Transform Start;
    public Transform End;
    private bool isWalking = false;
    private float speed;
    private Vector3 dir;

    private AudioSource audioSource;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        speed = (Start.position - End.position).magnitude / 3.0f;
        dir = (End.position - Start.position).normalized;
        transform.position = Start.position;
    }

    private void Update()
    {
        if (isWalking)
        {
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
        }
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

    public void OnLevelStart()
    {
        transform.position = Start.position;
        isWalking = true;
        animator.SetBool("Walking", isWalking);
        Invoke("StopWalking", 3.0f);
    }

    private void StopWalking()
    {
        isWalking = false;
        animator.SetBool("Walking", isWalking);
    }

}
