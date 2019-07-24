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

    public void OnCollectGarbage(bool isPickedRight)
    {
        animator.SetTrigger("Collect");
        animator.SetBool("PickingStatus", isPickedRight);
    }

    public void OnSmashed()
    {
        animator.SetTrigger("Smashed");
    }

}
