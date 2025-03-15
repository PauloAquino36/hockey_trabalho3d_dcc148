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
    public float rotationSmoothSpeed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        move();
        // Faz a rotação do player ser gradual
        if (shouldRotate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {  
            animator.SetBool("run", true);
            moviment(direction);
        }
        else
        {
            animator.SetBool("run", false);
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
}
