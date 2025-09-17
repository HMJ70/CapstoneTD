using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Pathway currpathway;

    private Vector3 targetPlace;

    private void Awake()
    {
        currpathway = GameObject.Find("Pathway").GetComponent<Pathway>();
    }


    private void OnEnable()
    {
        targetPlace = currpathway.GetPos(2);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPlace, speed * Time.deltaTime);
    }


}
