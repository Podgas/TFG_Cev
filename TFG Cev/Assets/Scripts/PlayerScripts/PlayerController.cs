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
    Transform aimCameraPosition;
    [SerializeField]
    Transform cameraAnchor;
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
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    public Transform bulletSpawn;

    //------------Move Vars ---------------//
    [Header("Movement")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private Transform model;
    [SerializeField]
    CharacterController cc;

    private float speed;
    private Vector3 _moveDirection;
    Vector3 joystickDirection;
    public Vector3 forward;
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

    //---------Dash Vars---------------//

    [Header("Dash")]
    [SerializeField]
    private float maxDashTime = 1.0f;
    [SerializeField]
    private float dashSpeed = 1.0f;
    [SerializeField]
    private float dashStoppingSpeed = 0.1f;

    private float currentDashTime;

    //--------Interaction Vars----------//

    [Header("Interaction")]
    [SerializeField]
    private LayerMask interactionLayer;
    [SerializeField]
    private LayerMask obstacleMask;
    [SerializeField]
    public float viewRadius;
    [SerializeField]
    [Range(0, 360)]
    public float viewAngle;

    public List<Transform> interactableTargets = new List<Transform>();




    //----------Attack Vars------------------//

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
    bool isShooting = false;
    bool isDashing = false;


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

        RecalculatePivot(cameraPosition);
        speed = walkSpeed;
    
    }



    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            FindVisibleInteractObjects();
        }
        
        IsGrounded();

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
            anim.SetBool("isMoving", isMoving);
        }
        else
        {
            isMoving = false;
            anim.SetBool("isMoving", isMoving);
        }
        Move(forward, right);

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

        

        cc.Move(_moveDirection* dashSpeed * Time.deltaTime);

    }


    void MainUpdate()
    {
        if (Input.GetAxis("Axis-3") != 0 || Input.GetAxis("Axis-4") != 0 && !isDashing)
        {
            RecalculatePivot(cameraPosition);
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

        //////////////////////////////////////////

        if (isMoving || Input.GetAxis("Axis-4") != 0)
        {
            if (joystickDirection != Vector3.zero) { }
            model.rotation = Quaternion.LookRotation(joystickDirection);

        }

        Dash();
        Interact();

    }

    void AimUpdate()
    {
        if (Input.GetAxis("Axis-4") != 0)
        {
            RotatePlayer();
        }
        /*

        if (Input.GetAxisRaw("Shoot") != 0)
        {
            if (isShooting == false)
            {
                Shoot();
                isShooting = true;  
            }
        }
        if (Input.GetAxisRaw("Shoot") == 0)
        {
            isShooting = false;
        }
        */
        if (isMoving ||
            (playerCondition.GetCondition() == PlayerCondition.Conditions.Aim && Input.GetAxis("Axis-4") != 0))
        {
            model.rotation = Quaternion.LookRotation(forward);

        }
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

    void RecalculatePivot(Transform camPosition)
    {
        forward = transform.position - camPosition.position;
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

    }
    void Shoot()
    {
        Vector3 instancePosition = bulletSpawn.position;
       

        Instantiate(bulletPrefab, instancePosition, Quaternion.identity);
    }

    void RotatePlayer()
    {
        RecalculatePivot(aimCameraPosition);

    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            
            currentDashTime = 0.0f;
        }
        if (currentDashTime < maxDashTime && isGrounded)
        {
            dashSpeed = 5;
            currentDashTime += dashStoppingSpeed;
            isDashing = true;
        }
        else
        {
            isDashing = false;
            dashSpeed = 1;
        }
    }

    void Interact()
    {

        if (Input.GetButtonDown("Interact"))
        {


          
        }

    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += model.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void FindVisibleInteractObjects()
    {

        interactableTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, interactionLayer);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(model.forward,dirToTarget)< viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    target.GetComponent<IInteractObjects>().OnInteract(); ;
                }
            }
        }
    }

}
