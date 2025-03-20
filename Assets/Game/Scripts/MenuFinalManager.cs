using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuFinalManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoPlacarFinal;
    [SerializeField] private string nomeCenaJogo; // Nome da cena do jogo para reiniciar

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Recupera os valores salvos no PlayerPrefs
        string vencedor = PlayerPrefs.GetString("Vencedor", "Partida Encerrada");
        int placarJogador1 = PlayerPrefs.GetInt("PlacarJogador1", 0);
        int placarJogador2 = PlayerPrefs.GetInt("PlacarJogador2", 0);

        // Atualiza o texto do placar final na tela
        if (textoPlacarFinal != null)
        {
            textoPlacarFinal.text = vencedor + "\nPlacar Final:\nVOCE: " + placarJogador1 + "  -  OPONENTE: " + placarJogador2;
        }
    }

    public void JogarNovamente()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void SairJogo()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
