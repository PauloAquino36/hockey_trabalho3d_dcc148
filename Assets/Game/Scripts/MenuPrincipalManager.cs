using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDeJogo;

    public void Jogar() // Agora é público
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(nomeDoLevelDeJogo);
    }

    public void SairJogo() // Agora é público
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
}
