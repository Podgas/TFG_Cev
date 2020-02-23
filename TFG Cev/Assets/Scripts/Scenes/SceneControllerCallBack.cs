using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerCallBack : MonoBehaviour
{

    private bool isFirstUpdate = true;
    private void Start()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            StartCoroutine(FakeLoading());
        }
    }

    IEnumerator FakeLoading()
    {

        yield return new WaitForSeconds(1.5f);

        SceneController.LoaderCallBack();
    }
}
