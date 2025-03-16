using UnityEngine;
using TMPro;

public class Placar : MonoBehaviour
{
    public int pontuacaoJogador1 = 0;
    public int pontuacaoJogador2 = 0;

    public TextMeshProUGUI textoPlacarJogador1;
    public TextMeshProUGUI textoPlacarJogador2;
////
    public Disco disco;
    public Player player1;
    public Mob player2;

    public void AtualizarPlacar(string jogadorTag)
    {
        if (jogadorTag == "Player1")
        {
            pontuacaoJogador1++;
            textoPlacarJogador1.text = pontuacaoJogador1.ToString();
        }
        else if (jogadorTag == "Player2")
        {
            pontuacaoJogador2++;
            textoPlacarJogador2.text = pontuacaoJogador2.ToString();
        }

        if (disco != null)
        {
            disco.resetPosition();
        }

        if (player1 != null)
        {
            player1.resetPosition();
        }

        if (player2 != null)
        {
            player2.resetPosition();
        }

        Debug.Log("Placar: Jogador 1 " + pontuacaoJogador1 + " x " + pontuacaoJogador2 + " Jogador 2");
    }
}