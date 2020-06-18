using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    EventSystem es;
    [SerializeField]
    CanvasGroup pausePanel;
    [SerializeField]
    CanvasGroup optionPanel;
    [SerializeField]
    Button pauseFirstButton;

    static bool isPaused;


    private void Awake()
    {
        pausePanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (isPaused)
            {
                
                isPaused = false;
                pausePanel.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                es.SetSelectedGameObject(pauseFirstButton.gameObject);
                pauseFirstButton.OnSelect(null);
                pauseFirstButton.Select();
                isPaused = true;
                pausePanel.gameObject.SetActive(true);
                
                Time.timeScale = 0f;
            }
            
        }
    }

    public void OnResume()
    {
        isPaused = false;
        pausePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public static bool GetPaused()
    {
        return isPaused;
    }

    public void OpenOptions()
    {

        optionPanel.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(false);
    }
    public void OnRetry()
    {
        SceneController.LoadScene(SceneController.CurrentScene(),true);
    }

    

    
}
