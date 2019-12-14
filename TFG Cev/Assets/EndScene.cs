using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{

    [SerializeField]
    PlayerStats playerStats;
    [SerializeField]
    GameObject winPanel;
    [SerializeField]
    GameObject losePanel;

    private void Start()
    {
        if (playerStats.win)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }
}
