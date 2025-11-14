using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickAnywhereToNextTutorial : MonoBehaviour
{
    [Header("Use Scene Name OR Level Manager")]
    public bool useLevelManager = false;

    [Header("If using SceneManager")]
    public string nextSceneName;

    [Header("If using lvlmanager")]
    public int nextLevelIndex;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (useLevelManager)
            {

                lvlmanager.instance.loadlevel(lvlmanager.instance.alllevels[nextLevelIndex]);
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
