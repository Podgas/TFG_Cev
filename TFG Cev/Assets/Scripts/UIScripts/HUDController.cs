using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField]
    private MapController mc;
    [SerializeField]
    private Text collectableText;
    [SerializeField]
    private GameObject tutorialPanel;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private TextMeshProUGUI quest;


    private void Start()
    {
        hpBar.fillAmount = playerStats.hp.value / playerStats.hp.maxValue;

        //collectableText.text = "0/" + mc.extras.Count.ToString();
    }

    public void UpdateHpBar(float percentage) {
        Debug.Log("Changing");
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
    public void UpdateCollectables()
    {
        collectableText.text = mc.completedExtras.Count.ToString() + "/" + mc.extras.Count.ToString();
    }

    public void OnTutorialEnter(GameObject tutorial)
    {
        tutorialPanel.SetActive(true);
    }
    public void OnTutorialExit(GameObject tutorial)
    {
        tutorialPanel.SetActive(false);
    }

    public void OnCatch()
    {
        losePanel.SetActive(true);
        playerStats.canMove = false;
    }
    public void OnRestartGame()
    {
        SceneController.LoadScene(SceneController.CurrentScene(), true);
        
    }
    public void OnQuestEnter(GameObject entredQuest)
    {

        MapEvent me;

        me = entredQuest.gameObject.GetComponent<MapEvent>();

        ChangeQuest(me.questLine);
        
    }

    public void ChangeQuest(string text)
    {
        quest.text = text;
    }
}
