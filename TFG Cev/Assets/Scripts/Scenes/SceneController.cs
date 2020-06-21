using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{

    public enum Scene
    {
        LogoScreen,
        TitleScreen,
        LoadingScene,
        Tutorial_town,
        Fortess,
        FinalScene
    }

    private static Action onLoaderCallBack;


    public static void LoadScene(Scene scene, bool loadingScene)
    {
        if (loadingScene)
        {
            onLoaderCallBack = () =>
            {
                SceneManager.LoadScene(scene.ToString());
            };

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }
        else
        {
            SceneManager.LoadScene(scene.ToString());
        }
        

    }
    public static void LoadScene(String scene, bool loadingScene)
    {
        if (loadingScene)
        {
            onLoaderCallBack = () =>
            {
                SceneManager.LoadScene(scene);
            };

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }
        else
        {
            SceneManager.LoadScene(scene);
        }


    }

    public static void LoaderCallBack()
    {
        if (onLoaderCallBack != null)
        {
            onLoaderCallBack();
            onLoaderCallBack = null;
        }
    }

    public static int GetActualScreenIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public static string GetActualScreenName()
    {
        return SceneManager.GetActiveScene().name;

    }

    public static string CurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }





}
