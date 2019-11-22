using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private Image hpBar;


    private void Start()
    {
        hpBar.fillAmount = playerHP.value / playerHP.maxValue;
    }

    public void UpdateHpBar(float percentage) {

        hpBar.fillAmount = percentage;

    }

}
