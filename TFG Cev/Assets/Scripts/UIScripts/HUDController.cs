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
    [SerializeField]
    private GameObject cross;
    [SerializeField]
    private Text ammoText;


    private void Start()
    {
        hpBar.fillAmount = playerStats.hp.value / playerStats.hp.maxValue;
        ammoText.text = playerStats.ammo + "/" + playerStats.maxAmmo;
    }

    public void UpdateHpBar(float percentage) {

        hpBar.fillAmount = percentage;

    }
    public void UpdateAmmoText()
    {
        ammoText.text = playerStats.ammo + "/" + playerStats.maxAmmo;
    }
    public void DisplayCross(PlayerCondition.Conditions condition)
    {

        if(condition == PlayerCondition.Conditions.Aim)
        {
                cross.SetActive(true);
        }
        else
        {
            cross.SetActive(false);
        }

    }

}
