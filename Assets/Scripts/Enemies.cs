using UnityEngine;
using System;
public class Enemies : MonoBehaviour
{
    [SerializeField] private EData data;

    public static event Action<EData> OnReachingBase;
    public static event Action<Enemies> OnEnemyKilled;

    private Pathway currpathway;

    private Vector3 targetPlace;

    private int currWayP;

    private float HP;

    private void Awake()
    {
        currpathway = GameObject.Find("Pathway").GetComponent<Pathway>();
    }


    private void OnEnable()
    {
        currWayP = 0;
        targetPlace = currpathway.GetPos(currWayP);
        HP = data.hp;
    }

    void Update()
    {
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
                OnReachingBase?.Invoke(data);
                gameObject.SetActive(false);
            }

        }
    }

    public void TakeDMG(float dmg)
    {
        HP -= dmg;
        HP = Math.Max(HP,0);
        if(HP <= 0)
        {
            OnEnemyKilled?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
