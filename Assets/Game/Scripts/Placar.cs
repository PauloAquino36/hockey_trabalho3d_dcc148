using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Placar : MonoBehaviour
{
    public int pontosParaVitoria = 3;
    public int pontuacaoJogador1 = 0;
    public int pontuacaoJogador2 = 0;

    public TextMeshProUGUI textoPlacarJogador1;
    public TextMeshProUGUI textoPlacarJogador2;

    public Disco disco;
    public Player player1;
    public Mob player2;

    private string vencedor = "";

    public void AtualizarPlacar(string jogadorTag)
    {
        if (jogadorTag == "Player1")
        {
            pontuacaoJogador1++;
            textoPlacarJogador1.text = pontuacaoJogador1.ToString();

            if (pontuacaoJogador1 >= pontosParaVitoria)
            {
                vencedor = "VOCE VENCEU!";
                IrParaMenuFinal();
                return;
            }
        }
        else if (jogadorTag == "Player2")
        {
            pontuacaoJogador2++;
            textoPlacarJogador2.text = pontuacaoJogador2.ToString();

            if (pontuacaoJogador2 >= pontosParaVitoria)
            {
                vencedor = "O OPONENTE VENCEU!";
                IrParaMenuFinal();
                return;
            }
        }

        ResetarPosicoes();

        Debug.Log("Placar: Jogador 1 " + pontuacaoJogador1 + " x " + pontuacaoJogador2 + " Jogador 2");
    }

    private void ResetarPosicoes()
    {
        if (disco != null)
            disco.resetPosition();

        if (player1 != null)
            player1.resetPosition();

        if (player2 != null)
            player2.resetPosition();
    }

    private void IrParaMenuFinal()
    {
        PlayerPrefs.SetString("Vencedor", vencedor);
        PlayerPrefs.SetInt("PlacarJogador1", pontuacaoJogador1);
        PlayerPrefs.SetInt("PlacarJogador2", pontuacaoJogador2);

        SceneManager.LoadScene("MenuFinal");
    }
}
