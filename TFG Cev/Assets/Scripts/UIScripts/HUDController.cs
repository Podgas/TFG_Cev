﻿using System.Collections;
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

    [SerializeField]
    List<GameObject> tutorialPanels;

    [SerializeField]
    List<GameObject> objetiveIcons;

    bool isChangingQuest;
    string newQUest;
    float currentTime = 0;

    [SerializeField]
    List<GameObject> SakePlates;
    private void Start()
    {
        hpBar.fillAmount = playerStats.hp.value / playerStats.hp.maxValue;

        //collectableText.text = "0/" + mc.extras.Count.ToString();
    }

    private void Update()
    {
        if (isChangingQuest)
        {
            currentTime += Time.deltaTime;

            if (currentTime > 0.1f)
            {
                SubLetter();
                currentTime = 0;
            }
        }
            
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

        switch (tutorial.name)
        {

            case "Walk":
                tutorialPanels[0].SetActive(true);
                break;
            case "Jump":
                tutorialPanels[1].SetActive(true);
                break;
            case "Run":
                tutorialPanels[2].SetActive(true);
                break;
            case "Climb":
                tutorialPanels[3].SetActive(true);
                break;
            case "Stealth":
                tutorialPanels[4].SetActive(true);
                break;
            case "Combat":
                tutorialPanels[4].SetActive(true);
                break;
        }
    }
    public void OnTutorialExit(GameObject tutorial)
    {
        switch (tutorial.name)
        {

            case "Walk":
                tutorialPanels[0].SetActive(false);
                break;
            case "Jump":
                tutorialPanels[1].SetActive(false);
                break;
            case "Run":
                tutorialPanels[2].SetActive(false);
                break;
            case "Climb":
                tutorialPanels[3].SetActive(false);
                break;
            case "Stealth":
                tutorialPanels[4].SetActive(false);
                break;
            case "Combat":
                tutorialPanels[4].SetActive(false);
                break;
        }
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
    public void OnQuestEnter(GameObject enteredQuest)
    {
        MapEvent me;
        me = enteredQuest.gameObject.GetComponent<MapEvent>();
        isChangingQuest = true;
        newQUest = me.questLine;
    }

    public void SubLetter()
    {
        string currentQuest = quest.text;
        int size = currentQuest.Length;
        currentQuest = currentQuest.Substring(0, currentQuest.Length - 1);
        quest.text = currentQuest;
        if (currentQuest.Length == 0)
        {
            
            isChangingQuest = false;
            ChangeQuest(newQUest);
        }
    }

    public void ChangeQuest(string text)
    {
        AudioManager.Instance.PlaySound("changeQuest");
        quest.text = text;
    }

    public void OnObjetiveComplete(float index)
    {
        switch (index)
        {
            case 0:

                objetiveIcons[0].SetActive(false);
                ChangeQuest("Use the medallion on the door");
                    
                break;
            case 1:

                objetiveIcons[1].SetActive(true);

                ChangeQuest("Escape from the fortress");
                break;
            case 2:
                ChangeQuest("Defeat Raksha");
                break;
        }
    }

    public void OnSake(float amount)
    {
        GameObject goToDeactivate = null;
        foreach(GameObject go in SakePlates)
        {
            if(amount > 0)
            {
                if (!go.active)
                    go.SetActive(true);
                break;
            }
            else
            {
                if (go.active)
                    goToDeactivate = go;
            }
        }
        if (goToDeactivate != null)
            goToDeactivate.SetActive(false);
    }
}
