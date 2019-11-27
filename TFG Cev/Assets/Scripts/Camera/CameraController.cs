using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCameraBase[] cameras;
    [SerializeField]
    PlayerCondition playerCondition;
    int currentCamera;

    private void Start()
    {
        currentCamera = 0;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }


        cameras[currentCamera].gameObject.SetActive(true);

    }

    void Update()
    {

        if (Input.GetAxis("Aim")>=0.5)
        {
            cameras[currentCamera].gameObject.SetActive(false);
            currentCamera = 1;
            cameras[currentCamera].gameObject.SetActive(true);
            ChangeCondition(PlayerCondition.Conditions.Aim);
        }
        else if(Input.GetButtonUp("Aim"))
        {
            cameras[currentCamera].gameObject.SetActive(false);
            currentCamera = 0;
            cameras[currentCamera].gameObject.SetActive(true);
            ChangeCondition(PlayerCondition.Conditions.Main);
        }

    }

    public void ChangeCondition(PlayerCondition.Conditions condition)
    {
        playerCondition.ChangeCondition(condition);
    }
}
