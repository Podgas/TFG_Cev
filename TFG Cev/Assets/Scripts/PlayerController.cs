using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //------------Move Vars ---------------//
    [Header("Movement")]
    [SerializeField]
    private float speed;
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

        //StickToWorldSpace(transform, cameraPosition, ref _moveDirection);       


    }

    //Moving the player 

    void Move()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
    }


    public void StickToWorldSpace(Transform root, Transform camera)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(_moveDirection.x, 0, _moveDirection.z);

        //get camera rotation
        Vector3 cameraDirection = camera.forward;
        cameraDirection.y = 0.0f;
        Quaternion reference = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        Vector3 moveDirection = reference * stickDirection;

        Quaternion.LookRotation(moveDirection);



        directionOut = moveDirection * angleRootToMove;

    }

}
