using UnityEngine;

public class Gol : MonoBehaviour
{
    public string jogadorTag;
    public Placar placar;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Disco"))
        {
            Debug.Log("Gol marcado por: " + jogadorTag);
            if (placar != null)
            {
                placar.AtualizarPlacar(jogadorTag);
            }
        }
    }
}