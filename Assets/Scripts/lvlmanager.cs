using UnityEngine;
using UnityEngine.SceneManagement;

public class lvlmanager : MonoBehaviour
{
    public static lvlmanager instance { get; private set; }

    public LVLData[] alllevels;
    public LVLData currlvl { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        currlvl = alllevels[0];
    }

    public void loadlevel(LVLData lvldata)
    {
        currlvl = lvldata;
        SceneManager.LoadScene(lvldata.lvlname);
    }

}
