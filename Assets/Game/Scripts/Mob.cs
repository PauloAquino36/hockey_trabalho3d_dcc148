using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float rotationSpeed = 720f;
    public float detectionRange = 10f; // Distância máxima para detectar o disco
    public float stoppingDistance = 1f; // Distância para parar perto do disco

    private Rigidbody rb;
    private Animator animator;
    private Quaternion targetRotation;
    private bool shouldRotate = false;
    private bool parado = false;
    public float rotationSmoothSpeed = 5f;
    private Vector3 posicaoInicial;
    private GameObject disco; // Referência ao objeto do disco

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        posicaoInicial = transform.position;
        disco = GameObject.FindGameObjectWithTag("Disco"); // Encontra o objeto do disco pela tag
    }

    void FixedUpdate()
    {
        if (!parado)
        {
            move();
        }

        // Faz a rotação do mob ser gradual
        if (shouldRotate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Verifica se a animação de colisão está ativa
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
        if (disco == null)
        {
            // Se o disco não for encontrado, não faz nada
            animator.SetInteger("run", 0); // Para a animação
            return;
        }

        // Calcula a direção para o disco
        Vector3 directionToDisco = disco.transform.position - transform.position;
        float distanceToDisco = directionToDisco.magnitude;

        // Se o disco estiver dentro do alcance de detecção
        if (distanceToDisco <= detectionRange)
        {
            // Rotaciona na direção do disco
            targetRotation = Quaternion.LookRotation(directionToDisco);
            shouldRotate = true;

            // Move-se em direção ao disco se estiver longe o suficiente
            if (distanceToDisco > stoppingDistance)
            {
                Vector3 movement = directionToDisco.normalized * walkSpeed * Time.deltaTime;
                rb.MovePosition(transform.position + movement);

                // Define a animação com base na direção do movimento
                bool isMovingBackward = Vector3.Dot(transform.forward, directionToDisco.normalized) < -0.5f;
                if (isMovingBackward)
                {
                    animator.SetInteger("run", 2); // Correndo para trás
                }
                else
                {
                    animator.SetInteger("run", 1); // Correndo para frente ou para os lados
                }
            }
            else
            {
                animator.SetInteger("run", 0); // Parado
            }
        }
        else
        {
            animator.SetInteger("run", 0); // Parado
        }
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