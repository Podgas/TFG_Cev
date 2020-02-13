using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{

    [SerializeField]
    Animator faderAnim;

    private SceneController.Scene levelToLoad;


    public void FadeToLevel(SceneController.Scene scene)
    {
        levelToLoad = scene;
        faderAnim.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneController.LoadScene(levelToLoad, false);
    }

    private void Update()
    {
        if(SceneController.GetActualScreenName() == SceneController.Scene.FinalScene.ToString())
        {
            if (Input.GetButton("Attack"))
            {
                FadeToLevel(SceneController.Scene.LogoScreen);
            }
        }
    }
}
