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
    [SerializeField]
    private PlayerConditionEvent onConditionChange;

    bool aimInUse = false;

    private void Start()
    {
        currentCamera = 0;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = 1;
        }


        cameras[currentCamera].Priority = 10;

    }

    void Update()
    {

        if (Input.GetAxisRaw("Aim") != 0)
        {
            if (aimInUse == false)
            {
                aimInUse = true;
                cameras[currentCamera].Priority = 1;
                currentCamera = 1;
                cameras[currentCamera].Priority = 10;
                ChangeCondition(PlayerCondition.Conditions.Aim);
            }
        }
        if (Input.GetAxisRaw("Aim") == 0 && aimInUse)
        {
            aimInUse = false;
            cameras[currentCamera].Priority = 1;
            currentCamera = 0;
            cameras[currentCamera].Priority = 10;
            ChangeCondition(PlayerCondition.Conditions.Main);
        }



    }

    public void ChangeCondition(PlayerCondition.Conditions condition)
    {
        playerCondition.ChangeCondition(condition);
        onConditionChange.Raise(condition);
    }
}
