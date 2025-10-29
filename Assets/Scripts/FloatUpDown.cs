using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float floatStrength = 0.5f; 
    public float floatSpeed = 2f;      

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatStrength;
    }
}
