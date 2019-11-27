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
    [SerializeField]
    PlayerCondition playerCondition;

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
    [SerializeField]
    CharacterController cc;

    private float speed;
    private Vector3 _moveDirection;
    Vector3 joystickDirection;
    Vector3 forward;
    Vector3 right;

    //------------Jump Vars ---------------//
    [Header("Jumping")]
    [SerializeField]
    float gravity = 2.5f;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    float hitDistance;
    [SerializeField]
    float fallMultiplier;
    [SerializeField]
    LayerMask groundHitLayer;

    //----------------------------//

    [Header("Attack")]
    [SerializeField]
    float timeToCharge;
    [SerializeField]
    float damageModifier;

    float damageDealt;
    float actualTime;


    //------------Bool State Vars ---------------//
    bool isGrounded = true;
    bool isMoving = false;
    bool isRunning = false;
    bool isJumping = false;
    bool isCharging = false;

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
            anim.SetBool("isMoving", isMoving);
        }

        switch (playerCondition.GetCondition())
        {
            case PlayerCondition.Conditions.Main:

                MainUpdate();
                break;
            case PlayerCondition.Conditions.Aim:
                AimUpdate();
                break;
        }

        if (!isGrounded)
        {
            if (isJumping)
                _moveDirection.y += Physics.gravity.y * gravity * Time.deltaTime;
            else
                _moveDirection.y += Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }

        cc.Move(_moveDirection * Time.deltaTime);


    }


    void MainUpdate()
    {
        if (Input.GetAxis("Axis-3") >= 0 || Input.GetAxis("Axis-4") >= 0)
        {
            RecalculatePivot();
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButton("Attack"))
        {
            Attack();
        }
        if (Input.GetButtonUp("Attack"))
        {
            isCharging = false;
            anim.SetBool("isCharging", isCharging);

            damageDealt += stats.baseDamage;
            actualTime = 0;
            onAttackLaunch.Raise();
        }


        /*DEBUGS*/

        Debug.DrawRay(transform.position, forward, Color.blue);
        Debug.DrawRay(transform.position, right, Color.green);
        Debug.DrawRay(transform.position, transform.up, Color.red);
        Debug.Log(isGrounded);

        //////////////////////////////////////////

        if (isMoving)
        {
            if (joystickDirection != Vector3.zero) { }
            model.rotation = Quaternion.LookRotation(joystickDirection);

        }

    }

    void AimUpdate()
    {
        
    }

    void Move(Vector3 forward, Vector3 right )
    {
        float storeY = _moveDirection.y;
        if (isGrounded)
        {
            CalculateSpeed();
        }
        _moveDirection = (Input.GetAxisRaw("Horizontal") * right * speed) + (Input.GetAxisRaw("Vertical") * forward * speed);
        _moveDirection.y = storeY;
        joystickDirection = _moveDirection.normalized;
        joystickDirection.y = 0;


    }

    void Jump()
    {
        _moveDirection.y = jumpForce;
        isJumping = true;


    }

    void IsGrounded()
    {
        Vector3 raycastTrans = transform.position;
        raycastTrans.y += cc.center.y +0.1f;
        Debug.DrawLine(raycastTrans, raycastTrans + Vector3.down*hitDistance, Color.cyan);
        
        if (Physics.Raycast(raycastTrans, -transform.up, hitDistance,groundHitLayer))
        {
            isGrounded = true;
            if (isJumping)
            {
                isJumping = false;
            }
        }
        else
        {
            isGrounded = false;
        }
            
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
        playerCondition.ChangeCondition(PlayerCondition.Conditions.Main);
        stats.hp.value = stats.hp.maxValue;
        stats.baseDamage = 1;
    }

    void Attack()
    {
        isCharging = true;
        anim.SetBool("isCharging", isCharging);
        actualTime += Time.deltaTime;

        if(actualTime >= timeToCharge)
        {
            damageDealt = damageModifier;
        }
        //anim.SetTrigger("Attacked");


    }
    void Shot()
    {

    }

}
