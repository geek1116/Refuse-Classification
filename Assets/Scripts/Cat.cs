using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void CollectGarbage(bool isPickedRight)
    {
        animator.SetBool("PickedStatus", isPickedRight);
        animator.SetTrigger("Collect");
    }

    public void OnSmashed()
    {
        animator.SetTrigger("Smashed");
    }

}
