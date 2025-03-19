using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float rotationSpeed = 720f;
    public float detectionRange = 20f; // Distância máxima para detectar o disco
    public float stoppingDistance = 1f; // Distância para parar perto do disco
    public float campoMetade = 0f; // Define a metade do campo (eixo Z)

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

        // Verifica se o mob está na metade do campo oposta ao disco
        bool estaNaMetadeOposta = VerificarMetadeOposta();

        // Se o disco estiver dentro do alcance de detecção e o mob não estiver na metade oposta
        if (distanceToDisco <= detectionRange && !estaNaMetadeOposta)
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
            // Se o disco estiver fora do alcance ou o mob estiver na metade oposta, volta para a posição inicial
            VoltarParaPosicaoInicial();
        }
    }

    bool VerificarMetadeOposta()
    {
        // Verifica se o mob está na metade do campo oposta ao disco
        if (disco.transform.position.z > campoMetade && transform.position.z <= campoMetade)
        {
            return true; // Mob está na metade oposta
        }
        else if (disco.transform.position.z <= campoMetade && transform.position.z > campoMetade)
        {
            return true; // Mob está na metade oposta
        }
        return false; // Mob não está na metade oposta
    }

    void VoltarParaPosicaoInicial()
    {
        // Calcula a direção para a posição inicial
        Vector3 directionToInitial = posicaoInicial - transform.position;
        float distanceToInitial = directionToInitial.magnitude;

        // Se não estiver na posição inicial, move-se para lá
        if (distanceToInitial > 0.5f)
        {
            // Rotaciona na direção da posição inicial
            targetRotation = Quaternion.LookRotation(directionToInitial);
            shouldRotate = true;

            // Move-se em direção à posição inicial
            Vector3 movement = directionToInitial.normalized * walkSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);

            animator.SetInteger("run", 1); // Correndo para frente
        }
        else
        {
            // Quando chegar à posição inicial, para completamente
            rb.linearVelocity = Vector3.zero; // Reseta a velocidade
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
        animator.SetInteger("run", 0); // Garante que a animação esteja parada
    }
}