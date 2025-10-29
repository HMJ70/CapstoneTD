using UnityEngine;

[CreateAssetMenu(fileName = "LVLData", menuName = "Scriptable Objects/LVLData")]
public class LVLData : ScriptableObject
{
    public string lvlname;
    public int wavestowin;
    public int startingcoins;
    public int startingHP;

    public WData[] waves;
}
