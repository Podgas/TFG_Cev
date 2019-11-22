using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    //------------Components Vars ---------------//
    [Header("Components")]
    [SerializeField]
    Transform cameraPosition;
    [SerializeField]
    Rigidbody rb;

    //------------Stats Vars ---------------//
    [Header("Stats")]
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private FloatEvent onHPChange;

    //------------Combat Vars ---------------//
    [Header("Combat")]
    [SerializeField]
    private VoidEvent onAttackLaunch;
    [SerializeField]
    private VoidEvent onAttackStops;

    //------------Move Vars ---------------//
    [Header("Movement")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    Transform model;

    private float speed;
    private Vector3 _moveDirection;
    Vector3 joystickDirection;
    Vector3 forward;
    Vector3 right;


    [Header("Jumping")]
    [SerializeField]
    float fallMultiplier = 2.5f;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    float hitDistance = 0.6f;

    //----------------------------//

    //------------Bool State Vars ---------------//
    bool isGrounded = true;
    bool isMoving = false;
    bool isRunning = false;

    //------------Anim Vars ---------------//
    [Header("Animation")]
    [SerializeField]
    Animator anim;

    private void Awake()
    {
        InitStats();
    }
    void Start()
    {

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
            anim.SetBool("isMoving", isMoving);
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
            anim.SetBool("isMoving", isMoving);
        }

        if (isGrounded && Input.GetAxis("Jump") != 0)
        {
            Jump();
        }

        if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("Attacked");
            onAttackLaunch.Raise();
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
        Debug.DrawRay(transform.position, -transform.up, Color.cyan);
       
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

    void UpdateHp(float value)
    {
        stats.hp.value += value;
        float percentage = (stats.hp.value) / stats.hp.maxValue;
        if (percentage >= 1)
        {
            percentage = 1;
            stats.hp.value = stats.hp.maxValue;
        }
        else if (percentage <= 0)
        {
            percentage = 0;
            stats.hp.value = stats.hp.minValue;
        }

        Debug.Log("HPChange");
        onHPChange.Raise(percentage);

    }

    void InitStats()
    {
        stats.hp.value = stats.hp.maxValue;
    }

}
