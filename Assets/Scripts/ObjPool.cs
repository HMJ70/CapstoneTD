using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int PSize = 5;

    private List<GameObject> Pool;

    void Start()
    {
        Pool = new List<GameObject>();
        for(int i = 0; i < PSize; i++)
        {
            MakeNewObj();
        }
    }

    private GameObject MakeNewObj()
    {
        GameObject obj = Instantiate(Prefab, transform);
        obj.SetActive(false);

        Pool.Add(obj);
        return obj;
    }

    public GameObject GetPObj()
    {
        foreach(GameObject obj in Pool)
        {
            if(!obj.activeSelf)
            {
                return obj;
            }
        }
        return MakeNewObj();
    }

}
