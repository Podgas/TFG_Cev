using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup pausePanel;

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
                Cursor.visible = false;
            }
            else
            {

                isPaused = true;
                pausePanel.gameObject.SetActive(true);
                Time.timeScale = 0f;
                Cursor.visible = true;
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
    public void OnRetry()
    {
        SceneController.LoadScene(SceneController.CurrentScene(), true);
    }
}
  

    

