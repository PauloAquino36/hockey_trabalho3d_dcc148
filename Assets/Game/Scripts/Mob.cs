using UnityEngine;
using System.Collections;
using System;

public class Mob : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float rotationSpeed = 720f;

    private Rigidbody rb;
    private Animator animator;
    private Quaternion targetRotation;
    private bool shouldRotate = false;
    private bool parado = false;
    public float rotationSmoothSpeed = 5f;
    private Vector3 posicaoInicial;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        posicaoInicial = transform.position;
    }

    void Update()
    {
        if(!parado)
        {
            move();
        }
        
        if (shouldRotate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("colide"))
        {
            parado = true;
        }
        else
        {
            parado = false;
        }
    }

    void move()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Disco")
        {
            animator.SetTrigger("stop");
            parado = true;
        }
    }

    public void resetPosition()
    {
        transform.position = posicaoInicial;
        rb.linearVelocity = Vector3.zero;
    }
}
