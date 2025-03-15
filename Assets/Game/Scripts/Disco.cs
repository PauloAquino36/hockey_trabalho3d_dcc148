using UnityEngine;

public class Disco : MonoBehaviour
{
    public float velocidadeInicial = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(velocidadeInicial, 0, velocidadeInicial);
    }

    void Update()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * velocidadeInicial;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 direcaoOposta = (transform.position - collision.transform.position).normalized;
            rb.linearVelocity = direcaoOposta * velocidadeInicial;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            rb.linearVelocity = Vector3.Reflect(rb.linearVelocity, normal);
        }
    }
}