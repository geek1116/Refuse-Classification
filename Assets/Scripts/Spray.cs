using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    public float cd;

    private Animator animator;

    private float timer = 0.0f;
    private bool trigger;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer < 0.0f)
        {
            trigger = true;
            timer = cd;
            animator.SetTrigger("Shoot");
            Invoke("SetTrigger", 0.5f);
        }
    }

    private void SetTrigger()
    {
        trigger = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (trigger)
        {
            trigger = false;
            if (collision.CompareTag("Garbage"))
            {
                LevelManager.instance.OnSprayShoot(collision.gameObject);
            }

        }
    }

}
