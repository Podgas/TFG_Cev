using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCotroller : MonoBehaviour
{
    [SerializeField]
    PlayerStats palyerStats;

    public void OnJump()
    {
        palyerStats.playerStatus.jumpPressed = true;  
    }

    public void OnInteraction()
    {
        palyerStats.playerStatus.interactPressed = true;
    }
}
