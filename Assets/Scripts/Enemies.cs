using UnityEngine;
using System;
public class Enemies : MonoBehaviour
{
    [SerializeField] private EData data;
    public EData Dataz => data;
    public static event Action<EData> OnReachingBase;
    public static event Action<Enemies> OnEnemyKilled;

    private Pathway currpathway;

    private Vector3 targetPlace;

    private int currWayP;

    private float HP;

    private float maxhp;

    [SerializeField] private Transform hpbar;
    private Vector3 hpbarog;
    private bool counted = false;

    private void Awake()
    {
        currpathway = GameObject.Find("Pathway").GetComponent<Pathway>();
        hpbarog = hpbar.localScale;
    }


    private void OnEnable()
    {
        currWayP = 0;
        targetPlace = currpathway.GetPos(currWayP);
    }

    void Update()
    {
        if (counted) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPlace, data.speed * Time.deltaTime);

        float roughlength = (transform.position - targetPlace).magnitude;
        if (roughlength < 0.1f )
        {
            if(currWayP < currpathway.WayP.Length - 1)
            {
                currWayP++;
                targetPlace = currpathway.GetPos(currWayP);
            }
            else
            {
                counted = true;
                OnReachingBase?.Invoke(data);
                gameObject.SetActive(false);
            }

        }
    }

    public void TakeDMG(float dmg)
    {
        if (counted) return;
        HP -= dmg;
        HP = Math.Max(HP,0);
        hpbarchange();
        if (HP <= 0)
        {
            counted = true;
            OnEnemyKilled?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    private void hpbarchange()
    {
        float hpercent = HP / maxhp;
        Vector3 scale = hpbarog;
        scale.x = hpbarog.x * hpercent;
        hpbar.localScale = scale;
    }

    public void Initialize(float hpmult)
    {
        counted = false;
        maxhp = data.hp * hpmult;
        HP = maxhp;
        hpbarchange();
    }


}
