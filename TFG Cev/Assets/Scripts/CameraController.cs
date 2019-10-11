using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset = new Vector3(0f, 1.5f, 0f);
    [SerializeField]
    float rotateSpeed;

    float horizontal;

    private Vector3 lookDir;
    private Vector3 targetPosition;

    private Vector3 velocityCameraSmooth = Vector3.zero;
    [SerializeField]
    private float camSmoothDampTime = 0.1f;

    void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
       
    }

    void Update()
    {
        horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, horizontal, 0);
    }

    void LateUpdate()
    {

        Vector3 characterOffset = target.position + offset;

        lookDir = characterOffset - this.transform.position;
        lookDir.y = 0;
        lookDir.Normalize();
        Debug.DrawRay(this.transform.position, lookDir, Color.green);

        //Sets the target position for the camera with the offset
        targetPosition = target.position + target.up * distanceUp - lookDir * distanceAway;

        /*Debug.DrawRay(target.position, Vector3.up * distanceUp, Color.red);
        Debug.DrawRay(target.position, -1f* target.forward * distanceAway, Color.blue);
        Debug.DrawLine(target.position, targetPosition);*/

        //smoothing the current possition and the new position
        SmoothPosition(this.transform.position, targetPosition);
        

        transform.LookAt(target);
    }

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCameraSmooth, camSmoothDampTime);
    }
}

