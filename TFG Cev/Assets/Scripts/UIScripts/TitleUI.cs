using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TitleUI : MonoBehaviour
{
    CanvasGroup actualCanvas;
    CanvasGroup logo;
    CanvasGroup logoPanel;
    CanvasGroup mainMenu;
    CanvasGroup options;
    public EventSystem eventSystem;
    float fadeTransitionTime = 1.5f;
    [SerializeField]
    Animator animator;


    [Header("OptionMenu")]
    [SerializeField]
    Button gameplayButton;
    [SerializeField]
    Button graphicButton;
    [SerializeField]
    Button soundbutton;

    [SerializeField]
    CanvasGroup gameplayOptions;
    [SerializeField]
    CanvasGroup graphicOptions;
    [SerializeField]
    CanvasGroup soundOptions;

    


    private void Start()
    {

        switch (SceneController.GetActualScreenName())
        {

            case "TitleScreen":

                TitleStart();

                break;
        }

    }

    void TitleStart()
    {
        logoPanel = GameObject.Find("LogoPanel").GetComponent<CanvasGroup>();
        mainMenu = GameObject.Find("MainMenu").GetComponent<CanvasGroup>();
        options = GameObject.Find("OptionPanel").GetComponent<CanvasGroup>();


        actualCanvas = logoPanel;
        Debug.Log(logoPanel);

    }

    private void Update()
    {

        switch (SceneController.GetActualScreenName())
        {

            case "TitleScreen":

                TitleUpdate();

                break;

        }

        if (Input.GetButton("Cancel"))
        {
            if(actualCanvas == options)
            {
                SwitchCanvas(actualCanvas, mainMenu);
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            if (gameplayButton.gameObject == EventSystem.current.currentSelectedGameObject)
            {
                gameplayOptions.gameObject.SetActive(true);
                soundOptions.gameObject.SetActive(false);
                graphicOptions.gameObject.SetActive(false);
            }
            else if (graphicButton.gameObject == EventSystem.current.currentSelectedGameObject)
            {
                gameplayOptions.gameObject.SetActive(false);
                soundOptions.gameObject.SetActive(false);
                graphicOptions.gameObject.SetActive(true);
            }
            else if (soundbutton.gameObject  == EventSystem.current.currentSelectedGameObject)
            {
                gameplayOptions.gameObject.SetActive(false);
                soundOptions.gameObject.SetActive(true);
                graphicOptions.gameObject.SetActive(false);
            }
        }
        

    }

    void TitleUpdate()
    {
        //Debug.Log(actualCanvas);
        if (Input.anyKey && actualCanvas.name == "LogoPanel")
        {
            SwitchCanvas(actualCanvas, mainMenu);

        }

    }


    void SwitchCanvas(CanvasGroup actualCG, CanvasGroup nextCG)
    {

        animator.SetTrigger("fadeOut"+ actualCG.name);
    }


    public void OnStartClick()
    {
        SceneController.LoadScene(SceneController.Scene.Fortess, true);
    }
    public void OnOptionsClick()
    {
        SwitchCanvas(actualCanvas, options);
        
    }
    

}
