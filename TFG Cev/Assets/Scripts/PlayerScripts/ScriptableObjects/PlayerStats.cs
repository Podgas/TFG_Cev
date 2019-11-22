
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{

    //Hp//
    [System.Serializable]
    public class PlayerHP{

        public float maxValue;
        public float minValue = 0;
        public float value;
    }

    [SerializeField]
    public PlayerHP hp;
    public float baseDamage;

}
