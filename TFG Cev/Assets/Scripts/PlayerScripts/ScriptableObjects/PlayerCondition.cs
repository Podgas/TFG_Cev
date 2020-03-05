using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerCondition", menuName = "Player/Condition")]
public class PlayerCondition : ScriptableObject
{

    public enum Conditions
    {
        Main,
        Aim
    }

    private static Conditions currentCondition;

    public void ChangeCondition(Conditions condition)
    {
        currentCondition = condition;
    }

    public Conditions GetCondition()
    {
        return currentCondition;
    }
}
