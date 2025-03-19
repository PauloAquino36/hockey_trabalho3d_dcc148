using UnityEngine;

public class Disco : MonoBehaviour
{
    public float velocidadeInicial = 7f;
    public float taxaDesaceleracao = 0.05f;
    private bool move = false;
    private Rigidbody rb;
    private Vector3 posicaoInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        posicaoInicial = transform.position;
    }   
    void FixedUpdate()
    {
        if (move)
        {
            if (rb.linearVelocity.magnitude > velocidadeInicial)
            {
                rb.linearVelocity -= rb.linearVelocity.normalized * taxaDesaceleracao * Time.fixedDeltaTime;
            }
            else
            {
                rb.linearVelocity = rb.linearVelocity.normalized * velocidadeInicial;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Mob"))
        {
            Vector3 direcaoOposta = (transform.position - collision.transform.position).normalized;
            direcaoOposta.y = 0;
            rb.linearVelocity = direcaoOposta * velocidadeInicial * 4f;
            if (!move)
            {
                move = true;
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 direcaoOposta = (transform.position - collision.transform.position).normalized;
            direcaoOposta.y = 0;
            rb.linearVelocity = direcaoOposta * velocidadeInicial * 2f;
        }
    }

    public void resetPosition()
    {
        rb.linearVelocity = Vector3.zero;
        move = false;
        transform.position = posicaoInicial;
    }
}