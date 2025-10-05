using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playgame()
    {
        lvlmanager.instance.loadlevel(lvlmanager.instance.alllevels[0]);
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
