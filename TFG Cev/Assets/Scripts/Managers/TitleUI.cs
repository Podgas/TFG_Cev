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
        logo = GameObject.Find("Logo").GetComponent<CanvasGroup>();
        options = GameObject.Find("OptionPanel").GetComponent<CanvasGroup>();
        logoPanel.alpha = 1;
        mainMenu.alpha = 0;
        logo.alpha = 0;
        options.alpha = 0;
        StartCoroutine(FadeCanvas(logo, logo.alpha, 1, fadeTransitionTime));

        actualCanvas = logoPanel;


    }

    private void Update()
    {

        switch (SceneController.GetActualScreenName())
        {

            case "TitleScreen":

                TitleUpdate();

                break;

        }

    }

    void TitleUpdate()
    {

        if (Input.anyKey && actualCanvas.name == "LogoPanel")
        {
            SwitchCanvas(actualCanvas, mainMenu, 0.5f);

        }

    }


    void SwitchCanvas(CanvasGroup actualCG, CanvasGroup nextCG, float time)
    {
        StartCoroutine(SwitchCanvasCoroutine(actualCG, nextCG, time));
    }

    private IEnumerator FadeCanvas(CanvasGroup canvas, float start, float end, float time = 0.5f)
    {
        float timeStartLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartLerping;
        float percentageComplete = timeSinceStarted / time;

        while (true)
        {

            timeSinceStarted = Time.time - timeStartLerping;
            percentageComplete = timeSinceStarted / time;

            float currentVlaue = Mathf.Lerp(start, end, percentageComplete);

            canvas.alpha = currentVlaue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }

    }
    private IEnumerator SwitchCanvasCoroutine(CanvasGroup actualCG, CanvasGroup nextCG, float time)
    {
        StartCoroutine(FadeCanvas(actualCG, actualCG.alpha, 0, time));
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeCanvas(nextCG, nextCG.alpha, 1, time));

        actualCanvas = nextCG;

        //actualCanvas.GetComponentInChildren<Button>().Select();
        //eventSystem.SetSelectedGameObject(actualCanvas.GetComponentInChildren<Button>().gameObject);
    }


    public void OnStartClick()
    {
        SceneController.LoadScene(SceneController.Scene.TestScene, true);
    }
    public void OnOptionsClick()
    {
        SwitchCanvas(actualCanvas, options, 0.5f);

    }
    public void OnExitClick()
    {

    }

}
