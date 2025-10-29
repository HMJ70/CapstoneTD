using UnityEngine;

public class Audiomanage : MonoBehaviour
{
    public static Audiomanage instance { get; private set; }

    public AudioClip buttonclicklclip;
    public AudioClip buttonhoverclip;
    public AudioClip towerplacedclip;
    public AudioClip enemykilled;
    public AudioClip missioncomplete;
    public AudioClip gameoverclip;
    public AudioClip pauseclip;
    public AudioClip resumeclip;
    public AudioClip speedslowclip;
    public AudioClip speedfastclip;
    public AudioClip speednormalclip;
    public AudioClip paneltoggleclip;
    public AudioClip warningclip;


    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource msource;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void playsound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void playtowerplaced() => playsound(towerplacedclip);
    public void playenemykilled() => playsound(enemykilled);
    public void playbuttonclicked() => playsound(buttonclicklclip);
    public void playbuttonhover() => playsound(buttonhoverclip);
    public void playmissioncomplete() => playsound(missioncomplete);
    public void playgameover() => playsound(gameoverclip);
    public void playresume() => playsound(resumeclip);
    public void playspeedslow() => playsound(speedslowclip);
    public void playspeednormal() => playsound(speednormalclip);
    public void playspeedfast() => playsound(speedfastclip);
    public void playpanel() => playsound(pauseclip);
    public void playwarning() => playsound(warningclip);
    public void playpause() => playsound(pauseclip);
}