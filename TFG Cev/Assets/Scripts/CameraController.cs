using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{

    //Variables for camera position
    [Header("Camera Position")]
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private bool useOffset;
    [SerializeField]
    private Transform target;

    private Transform xForm_Camera;
    private Transform xForm_CameraPivot;
    private Vector3 localRotation;
    



    //Movement Vars
    [Header("Camera Movement")]
    
    [SerializeField]
    private Transform anchor;
    [SerializeField]
    private float maxViewAngle;
    [SerializeField]
    private float minViewAngle;
    [SerializeField]
    private float smoothSpeed;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float orbitDamp;
    [SerializeField]
    private float orbitSpeed;

    public Vector3 lookDirection;
    [SerializeField]
    private float _CameraDistance = 10f;




    // Start is called before the first frame update
    void Start()
    {
        //if we use offset, we will use the Vector3 defined on inspector for the cmaera position
        if (!useOffset)
        {
            offset = target.position - transform.position;
        }

        anchor = GameObject.FindGameObjectWithTag("CameraAnchor").transform;
        //Copy the position of the target to the anchor and set Anchor free
        anchor.transform.position = target.transform.position;
        anchor.transform.parent = null;

        xForm_Camera = this.transform;
        xForm_CameraPivot = this.transform.parent;

    }

    // Update is called once per frame
    void LateUpdate()
    {

        anchor.transform.position = target.transform.position;


        if(Input.GetAxis("Axis-3") != 0 || Input.GetAxis("Axis-4") != 0)
        {
            localRotation.x += Input.GetAxis("Axis-4") * orbitSpeed;
            localRotation.y += Input.GetAxis("Axis-3") * -orbitSpeed;

            localRotation.y = Mathf.Clamp(localRotation.y, minViewAngle, maxViewAngle);

            if(Input.GetAxis("Axis-3") > 0 || Input.GetAxis("Axis-3") <= 0)
            {
                float scrollAmount = Input.GetAxis("Axis-3");

                scrollAmount *= (xForm_Camera.position.z * 0.3f);

                _CameraDistance += scrollAmount * -1f;

                

            }
        }



        Quaternion QT = Quaternion.Euler(localRotation.y, localRotation.x, 0);

        xForm_CameraPivot.rotation = Quaternion.Lerp(xForm_CameraPivot.rotation, QT, Time.deltaTime * orbitDamp);
        xForm_CameraPivot.position = Vector3.MoveTowards(xForm_CameraPivot.position, anchor.position, Time.time);

        transform.LookAt(target);

    }

}
