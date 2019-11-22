using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private Image hpBar;


    private void Start()
    {
        hpBar.fillAmount = playerStats.hp.value / playerStats.hp.maxValue;
    }

    public void UpdateHpBar(float percentage) {

        hpBar.fillAmount = percentage;

    }

}
