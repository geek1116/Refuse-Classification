using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    public float radius = 3;
    private float speed;
    private Vector3 dir;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        speed = 1.0f;
        dir = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
        animator.SetFloat("Speed", speed);
        if(Mathf.Abs(transform.position.x) > radius)
        {
            speed = -speed;
        }
    }
}
