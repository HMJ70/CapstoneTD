using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playgame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
