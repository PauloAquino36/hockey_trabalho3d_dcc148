using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 8f;
    public Transform cameraTransform;
    public float rotationSpeed = 720f;
    public float mouseSensitivity = 1000f;

    private Rigidbody rb;
    private Animator animator;
    private bool parado = false;
    private Vector3 posicaoInicial;
    private float yRotation = 0f; // Rotação horizontal da câmera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        posicaoInicial = transform.position;
        
        // Esconde e trava o cursor
        LockAndHideCursor();
    }

    void Update()
    {

        // Verifica se o jogo está em foco e trava o cursor novamente, se necessário
        if (Cursor.lockState != CursorLockMode.Locked || Cursor.visible)
        {
            LockAndHideCursor();
        }

        // Controle da rotação da câmera com o mouse (apenas horizontal)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f); // Aplica a rotação ao player
    }

    void FixedUpdate()
    {
        if (!parado)
        {
            move();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("colide"))
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
        // Movimento relativo à rotação do player (câmera)
        Vector3 moveDirection = transform.forward * direction.z + transform.right * direction.x;
        moveDirection.Normalize();

        Vector3 newVelocity = moveDirection * walkSpeed;
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);

        // Define a animação com base na direção do movimento
        if (direction.z < 0 && Mathf.Abs(direction.x) < 0.1f) // Movendo para trás
        {
            animator.SetInteger("run", 2);
        }
        else if (direction.magnitude > 0.1f) // Movendo para frente ou para os lados
        {
            animator.SetInteger("run", 1);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Disco")
        {
            animator.SetTrigger("stop");
        }
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void resetPosition()
    {
        transform.position = posicaoInicial;
        rb.linearVelocity = Vector3.zero;
    }
}