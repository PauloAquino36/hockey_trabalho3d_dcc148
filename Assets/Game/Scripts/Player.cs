using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Transform cameraTransform;
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate ()
    {
        if(!parado)
        {
            move();
        }
        // Faz a rotação do player ser gradual
        if (shouldRotate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("colide"))
        {
            animator.SetInteger("run", 0);
            parado = true;
        }
        else
        {
            parado = false;
        }
    }

    void move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {  
            moviment(direction);
        }
        else
        {
            animator.SetInteger("run", 0);
        }
    }
    void moviment(Vector3 direction)
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * direction.z + right * direction.x).normalized;
        Vector3 newVelocity = moveDirection * walkSpeed;

        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);

        // Verifica se o jogador está apenas andando para trás (S) sem pressionar esquerda/direita
        bool onlyMovingBackward = direction.z < 0 && Mathf.Abs(direction.x) < 0.1f;
        if(onlyMovingBackward)
        {
            animator.SetInteger("run", 2);
        }
        else
        {
            animator.SetInteger("run", 1);
        }

        if (!onlyMovingBackward && moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
        }
    }

    void AlignCameraWithPlayer()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        targetRotation = Quaternion.LookRotation(cameraForward);
        shouldRotate = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Disco")
        {
            animator.SetTrigger("stop");
        }
    }

    public void resetPosition()
    {
        transform.position = posicaoInicial;
        rb.linearVelocity = Vector3.zero;
    }
}
