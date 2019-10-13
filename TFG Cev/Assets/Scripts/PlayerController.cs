using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //------------Move Vars ---------------//
    [Header("Movement")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private float DirectionDampTime = .25f;

    private Vector3 _moveDirection;

    Transform myTransform;

    Transform cameraPosition;

    void Start()
    {
        myTransform = transform;
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }


    void Update()
    {


        Move();

       

        myTransform.Translate(_moveDirection * speed * Time.deltaTime);
        if (Input.anyKey)
        {
            StickToWorldSpace(transform, cameraPosition);
        }
           


    }

    //Moving the player 

    void Move()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
        if(Input.GetAxis("Vertical")!= 0)
        {
            float newForward = (Mathf.LerpAngle(transform.rotation.eulerAngles.y, cameraPosition.rotation.eulerAngles.y, 7f * Time.deltaTime));
            transform.rotation = Quaternion.Euler(0f, newForward, 0f);
        }
    }


    public void StickToWorldSpace(Transform root, Transform camera)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(_moveDirection.x, 0, _moveDirection.z);

        //get camera rotation
        Vector3 cameraDirection = camera.forward;
        cameraDirection.y = 0.0f;
        Quaternion reference = Quaternion.FromToRotation(transform.forward, cameraDirection);

        Vector3 moveDirection = reference * stickDirection;


        Debug.DrawRay(transform.position, moveDirection, Color.blue);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        
        Quaternion actualRot = Quaternion.LookRotation(moveDirection);
        //transform.rotation = actualRot;
        //transform.rotation = actualRot;
        //directionOut = moveDirection * angleRootToMove;

    }

}
