using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    //------------Move Vars ---------------//
    [Header("Movement")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float DirectionDampTime = .25f;
    [SerializeField]
    private Transform bottomPosition;
    [SerializeField]
    Transform model;



    [Header("Jumping")]
    [SerializeField]
    float fallMultiplier = 2.5f;




    private Vector3 _moveDirection;
    Vector3 joystickDirection;

    Transform myTransform;
    Rigidbody rb;
    Transform cameraPosition;
    Transform cameraAnchor;
    private  CinemachineFreeLook CMFL;
    bool rotating = false;

    bool isGrounded = true;
    bool isMoving = false;
    bool isRunning = false;


    float hitDistance=1.05f;
    [SerializeField]
    LayerMask layer;

    Vector3 forward;
    Vector3 right;
    void Start()
    {
        myTransform = transform;
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cameraAnchor = GameObject.FindGameObjectWithTag("CameraAnchor").transform;
        CMFL = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineFreeLook>();
        rb = GetComponent<Rigidbody>();

        RecalculatePivot();
        speed = walkSpeed;
    }



    void Update()
    {

        RecalculatePivot();
        IsGrounded();

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
            Move(forward, right);
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
        }

        if (isGrounded && Input.GetAxis("Jump") != 0)
        {
            Jump();
        }



        if (!isGrounded)
        {
            Debug.Log("IsNotGroudned");
            _moveDirection.y += (Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime);
        }

        /*DEBUGS*/

        Debug.DrawRay(transform.position, forward, Color.blue);
        Debug.DrawRay(transform.position, right, Color.green);
        Debug.DrawRay(transform.position, transform.up, Color.red);

        //////////////////////////////////////////

        if (isMoving)
        {
            if(joystickDirection != Vector3.zero)
                model.rotation = Quaternion.LookRotation(joystickDirection);


            //myTransform.Translate(_moveDirection * Time.deltaTime, Space.Self);
        }
        else
        {
            _moveDirection.x = 0;
            _moveDirection.z = 0;
        }

        
        rb.velocity = _moveDirection;

    }

 

    void Move(Vector3 forward, Vector3 right )
    {
        if(isGrounded)
            CalculateSpeed();
        _moveDirection = (Input.GetAxisRaw("Horizontal") * right * speed) +  (Input.GetAxisRaw("Vertical") *  forward * speed);
        _moveDirection.y = rb.velocity.y;
        joystickDirection = _moveDirection;
        joystickDirection.y = 0;

        
    }

    void Jump()
    {

        if(rb.velocity.y <= 0)
        {
            _moveDirection += Vector3.up * jumpForce;
        }
        
    }

    void IsGrounded()
    {
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, 0.95f, 0), Color.cyan);
       
        if (Physics.Raycast(transform.position, -transform.up, hitDistance))
        {
            isGrounded = true;
            _moveDirection.y =0;
        }
            
        else
            isGrounded = false;
    }

    void RecalculatePivot()
    {
        forward = transform.position - cameraPosition.position;
        right = -Vector3.Cross(forward.normalized, transform.up.normalized);
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
    }
    void CalculateSpeed()
    {

        if(Input.GetAxis("Run") != 0 && !isRunning)
        {
            speed = runSpeed;
            isRunning = true;
        }
        else if(Input.GetAxis("Run")==0 && isRunning)
        {
            speed = walkSpeed;
            isRunning = false;
        }
    }


}
