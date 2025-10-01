using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] private EData data;

    private Pathway currpathway;

    private Vector3 targetPlace;

    private int currWayP;


    private void Awake()
    {
        currpathway = GameObject.Find("Pathway").GetComponent<Pathway>();
    }


    private void OnEnable()
    {
        currWayP = 0;
        targetPlace = currpathway.GetPos(currWayP);
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
                gameObject.SetActive(false);
            }

        }
    }


}
