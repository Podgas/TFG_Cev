
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHP")]
public class PlayerHP : ScriptableObject
{
    public float maxValue;
    public float minValue = 0;
    public float value;

}
