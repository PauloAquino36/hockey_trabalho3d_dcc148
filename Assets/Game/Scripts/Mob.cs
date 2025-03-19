using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float rotationSpeed = 720f;
    public float detectionRange = 20f; // Distância máxima para detectar o disco
    public float stoppingDistance = 1f; // Distância para parar perto do disco
    public float campoMetade = 0f; // Define a metade do campo (eixo Z)
    public float cooldownChute = 1f; // Tempo de espera entre chutes
    public Transform golAdversario; // Referência ao gol adversário
    public Transform golProprio; // Referência ao gol do mob
    public Transform paredeEsquerda; // Referência à parede esquerda
    public Transform paredeDireita; // Referência à parede direita

    private Rigidbody rb;
    private Animator animator;
    private Quaternion targetRotation;
    private bool shouldRotate = false;
    private bool parado = false;
    public float rotationSmoothSpeed = 5f;
    private Vector3 posicaoInicial;
    private GameObject disco; // Referência ao objeto do disco
    private bool podeChutar = true; // Controla se o mob pode chutar o disco

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        posicaoInicial = transform.position;
        disco = GameObject.FindGameObjectWithTag("Disco"); // Encontra o objeto do disco pela tag

        if (golAdversario == null || golProprio == null || paredeEsquerda == null || paredeDireita == null)
        {
            Debug.LogError("Gol adversário, gol próprio ou paredes laterais não atribuídos ao Mob!");
        }
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

        // Verifica se o disco está indo em direção ao gol do mob
        bool discoIndoParaGolProprio = DiscoIndoParaGolProprio();

        // Se o disco estiver dentro do alcance de detecção e o mob não estiver na metade oposta
        if (distanceToDisco <= detectionRange && !estaNaMetadeOposta)
        {
            // Se o disco estiver indo em direção ao gol do mob, persegue o disco para defendê-lo
            if (discoIndoParaGolProprio)
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
            // Se o mob puder chutar em direção ao gol adversário ou para as paredes laterais, persegue o disco
            else if (EstaChutandoParaGolAdversario() || EstaChutandoParaParedes())
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
                // Se não puder chutar em direção ao gol adversário ou para as paredes laterais, volta para a posição inicial
                VoltarParaPosicaoInicial();
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
        if (collision.gameObject.tag == "Disco" && podeChutar)
        {
            // Verifica se o mob está chutando em direção ao gol adversário, para as paredes laterais ou defendendo o gol próprio
            if (EstaChutandoParaGolAdversario() || EstaChutandoParaParedes() || DiscoIndoParaGolProprio())
            {
                animator.SetTrigger("stop");
                parado = true;

                // Aplica uma força no disco
                Vector3 direcaoOposta = (collision.transform.position - transform.position).normalized;
                direcaoOposta.y = 0;
                collision.rigidbody.AddForce(direcaoOposta * 10f, ForceMode.Impulse);

                // Inicia o cooldown do chute
                StartCoroutine(CooldownChute());
            }
            else
            {
                Debug.Log("Mob evitou chute em direção ao próprio gol.");
            }
        }
    }

    bool EstaChutandoParaGolAdversario()
    {
        if (golAdversario == null)
        {
            Debug.LogError("Gol adversário não atribuído ao Mob!");
            return false;
        }

        // Calcula a direção do chute (direção do mob para o disco)
        Vector3 direcaoChute = disco.transform.position - transform.position;
        direcaoChute.Normalize();

        // Calcula a direção do mob para o gol adversário
        Vector3 direcaoGolAdversario = golAdversario.position - transform.position;
        direcaoGolAdversario.Normalize();

        // Verifica se o chute está alinhado com a direção do gol adversário
        float produtoEscalar = Vector3.Dot(direcaoChute, direcaoGolAdversario);

        // Se o produto escalar for positivo, o chute está na direção do gol adversário
        return produtoEscalar > 0.7f; // Ajuste o valor conforme necessário
    }

    bool EstaChutandoParaParedes()
    {
        if (paredeEsquerda == null || paredeDireita == null)
        {
            Debug.LogError("Paredes laterais não atribuídas ao Mob!");
            return false;
        }

        // Calcula a direção do chute (direção do mob para o disco)
        Vector3 direcaoChute = disco.transform.position - transform.position;
        direcaoChute.Normalize();

        // Calcula a direção do mob para as paredes laterais
        Vector3 direcaoParedeEsquerda = paredeEsquerda.position - transform.position;
        direcaoParedeEsquerda.Normalize();

        Vector3 direcaoParedeDireita = paredeDireita.position - transform.position;
        direcaoParedeDireita.Normalize();

        // Verifica se o chute está alinhado com a direção de alguma das paredes
        float produtoEscalarEsquerda = Vector3.Dot(direcaoChute, direcaoParedeEsquerda);
        float produtoEscalarDireita = Vector3.Dot(direcaoChute, direcaoParedeDireita);

        // Se o produto escalar for positivo, o chute está na direção de uma das paredes
        return produtoEscalarEsquerda > 0.7f || produtoEscalarDireita > 0.7f; // Ajuste o valor conforme necessário
    }

    bool DiscoIndoParaGolProprio()
    {
        if (golProprio == null)
        {
            Debug.LogError("Gol próprio não atribuído ao Mob!");
            return false;
        }

        // Calcula a direção do disco para o gol próprio
        Vector3 direcaoDiscoParaGol = golProprio.position - disco.transform.position;
        direcaoDiscoParaGol.Normalize();

        // Calcula a direção do movimento do disco
        Vector3 direcaoMovimentoDisco = disco.GetComponent<Rigidbody>().linearVelocity.normalized;

        // Verifica se o disco está se movendo em direção ao gol próprio
        float produtoEscalar = Vector3.Dot(direcaoMovimentoDisco, direcaoDiscoParaGol);

        // Se o produto escalar for positivo, o disco está indo em direção ao gol próprio
        return produtoEscalar > 0.7f; // Ajuste o valor conforme necessário
    }

    IEnumerator CooldownChute()
    {
        podeChutar = false; // Impede novos chutes
        yield return new WaitForSeconds(cooldownChute); // Espera o tempo de cooldown
        podeChutar = true; // Permite novos chutes
    }

    public void resetPosition()
    {
        transform.position = posicaoInicial;
        rb.linearVelocity = Vector3.zero;
        animator.SetInteger("run", 0); // Garante que a animação esteja parada
    }
}