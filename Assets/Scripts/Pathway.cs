using UnityEngine;
using UnityEditor;

public class Pathway : MonoBehaviour
{
    public GameObject[] WayP;
    private LineRenderer line;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = WayP.Length;
        for (int i = 0; i < WayP.Length; i++)
        {
            line.SetPosition(i, WayP[i].transform.position);
        }
    }

    private void Update()
    {
        line.material.mainTextureOffset -= new Vector2(Time.deltaTime * 0.1f, 0);
    }
    public Vector3 GetPos(int index)
    {
        return WayP[index].transform.position;
    }

    private void OnDrawGizmos()
    {
        if(WayP.Length > 0)
        {
            for(int i = 0; i < WayP.Length; i++)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;
                Handles.Label(WayP[i].transform.position + Vector3.up * 0.7f, WayP[i].name, style);

                if(i < WayP.Length - 1)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(WayP[i].transform.position, WayP[i + 1].transform.position);
                }

            }
        }
    }


}
