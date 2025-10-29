using UnityEngine;

[CreateAssetMenu(fileName = "WData", menuName = "Scriptable Objects/WData")]
public class WData : ScriptableObject
{
    public EType Etype;
    public float Sinterval;
    public int EperWave;
    public AudioClip wavespawnclip;
}
