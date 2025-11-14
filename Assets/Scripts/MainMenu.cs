using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Title Settings")]
    public Transform title;
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 2f;
    public SpriteRenderer titleSprite;
    public Color colorA = Color.white;
    public Color colorB = Color.cyan;
    public float colorSpeed = 2f;
    private Vector3 titleStartPos;

    [Header("Fade Settings")]
    public Image fadeImage;         
    public float fadeDuration = 1f;  

    void Start()
    {
        if (title != null)
            titleStartPos = title.position;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.raycastTarget = false; 
        }
    }

    void Update()
    {
        AnimateTitle();
    }

    void AnimateTitle()
    {
        if (title == null) return;

        float y = titleStartPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        title.position = new Vector3(titleStartPos.x, y, titleStartPos.z);

        if (titleSprite != null)
        {
            float t = (Mathf.Sin(Time.time * colorSpeed) + 1f) / 2f;
            titleSprite.color = Color.Lerp(colorA, colorB, t);
        }
    }

    public void PlayGame()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = true; 

            float t = 0f;
            Color c = fadeImage.color;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        //lvlmanager.instance.loadlevel(lvlmanager.instance.alllevels[0]);
        SceneManager.LoadScene("Tutorial");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
