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
    [SerializeField]
    public Transform playerModel;
    [SerializeField]
    private Transform foxModel;
    [SerializeField]
    public Transform testModel;

    [HideInInspector]
    public Transform model;

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
    private float runFoxSpeed;
    [SerializeField]
    private float timeToMax;
    [SerializeField]
    private float timeToMin;
    [SerializeField]
    public CharacterController cc;

    private float speed;
    public Vector3 _moveDirection;
    Vector3 joystickDirection;
    public Vector3 forward;
    Vector3 right;
    public Vector3 dir;
    float accelRatePerSec;
    float decelRatePerSec;
    [SerializeField]
    float velocity;

    float playerHeight;

    //------------Jump Vars ---------------//
    [Header("Jumping")]
    [SerializeField]
    float gravity = 2.5f;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpFox;
    [SerializeField]
    float hitDistance;
    [SerializeField]
    float fallMultiplier;
    [SerializeField]
    LayerMask groundHitLayer;
    [SerializeField]
    public int jumpTimes;

    int jumpCount = 0;

    //---------Dash Vars---------------//

    [Header("Dash")]
    [SerializeField]
    private float maxDashTime = 1.0f;
    [SerializeField]
    private float dashSpeed = 3.0f;
    [SerializeField]
    private float dashStoppingSpeed = 0.1f;

    private float currentDashTime = 99f;

    Vector3 dashDirection;

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

    [SerializeField]
    FieldOfViewSystem fov;

    //--------Climb Vars----------//
    [SerializeField]
    private LayerMask climbMask;


    Transform carriedObject;


    //----------Attack Vars------------------//

    [Header("Attack")]
    [SerializeField]
    float timeToCharge;
    [SerializeField]
    float damageModifier;

    [HideInInspector]
    public float damageDealt;
    float actualTime;


    //------------Anim Vars ---------------//
    [Header("Animation")]
    [SerializeField]
    Animator anim;



    [SerializeField]
    LevelChanger levelChanger;
    [SerializeField]
    AudioLibrary vfx;

    float actualFootTime;
    float footTime = 0.5f;



    private void Awake()
    {
        InitStats();
        playerHeight = cc.height;
        playerModel.gameObject.SetActive(true);
        //foxModel.gameObject.SetActive(false);
        model = playerModel;
        
    }
    void Start()
    {
        Cursor.visible = false;
        RecalculatePivot(cameraPosition);
        speed = walkSpeed;
        accelRatePerSec = speed / timeToMax;
        decelRatePerSec = speed / timeToMin;
    }



    void Update()
    {
        
        if (!MenuManager.GetPaused() || !stats.playerStatus.isDead)
        {
            if (stats.playerStatus.isClimbing)
            {
                Climb();
            }
            else
            {
                if (Input.GetButtonDown("Transform")){
                    Debug.Log("Traaansform");
                    if (stats.playerStatus.isFox)
                    {
                        UnTransform();
                    }
                    else Transform();

                }

                //IsGrounded();
                stats.playerStatus.isGrounded = cc.isGrounded;

                if (stats.playerStatus.isGrounded && stats.playerStatus.isJumping)
                {
                    stats.playerStatus.isJumping = false;
                    jumpCount = 0;
                }

                

                Move(forward, right, dir);

                switch (playerCondition.GetCondition())
                {
                    case PlayerCondition.Conditions.Main:

                        MainUpdate();
                        break;
                    case PlayerCondition.Conditions.Aim:
                        AimUpdate();
                        break;
                    case PlayerCondition.Conditions.Fox:
                        FoxUpdate();
                        break;
                }

                if (!stats.playerStatus.isGrounded)
                {
                    if (stats.playerStatus.isJumping)
                        _moveDirection.y += Physics.gravity.y * gravity * Time.deltaTime;
                    else
                        _moveDirection.y += Physics.gravity.y * fallMultiplier * Time.deltaTime;
                }


                if(stats.playerStatus.isGrounded)
                {
                    actualFootTime += Time.deltaTime;
                    if (actualFootTime >= footTime)
                    {
                        actualFootTime = 0;
                        //vfx.PlayVFX(AudioLibrary.VfxSounds.Footstep,0.04f);
                    }
                }

                if (velocity !=0 || stats.playerStatus.isJumping)
                    cc.Move(_moveDirection * Time.deltaTime);
                
    
                
                if (Input.GetButtonDown("Interact") && !stats.playerStatus.isFox)
                {
                    FindVisibleInteractObjects();
                }
            }
        }
        


    }


        void MainUpdate()
    {
        if (Input.GetAxis("Axis-3") != 0 || Input.GetAxis("Axis-4") != 0 && !stats.playerStatus.isDashing)
        {
            RecalculatePivot(cameraPosition);
        }

        if ((stats.playerStatus.isGrounded || jumpCount < jumpTimes) && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButton("Attack"))
        {
            Attack();
        }
        if (Input.GetButtonUp("Attack"))
        {
            stats.playerStatus.isCharging = false;

            anim.SetBool("isCharging", stats.playerStatus.isCharging);

            damageDealt = stats.baseDamage + damageModifier;
            actualTime = 0;
            onAttackLaunch.Raise();

        }


        /*DEBUGS*/

        Debug.DrawRay(transform.position, forward, Color.blue);
        Debug.DrawRay(transform.position, right, Color.green);
        Debug.DrawRay(transform.position, transform.up, Color.red);

        //////////////////////////////////////////

        if (stats.playerStatus.isMoving)
        {
            if (!stats.playerStatus.isDashing)
            {
                model.rotation = Quaternion.Lerp(model.rotation, Quaternion.LookRotation(joystickDirection * 
                    Time.deltaTime), speed * Time.deltaTime);
            }
               

        }
        if(stats.playerStatus.isDashing || Input.GetButtonDown("Dash")){

            Dash();
        }

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
        if (stats.playerStatus.isMoving ||
            (playerCondition.GetCondition() == PlayerCondition.Conditions.Aim && Input.GetAxis("Axis-4") != 0))
        {
            if (!stats.playerStatus.isDashing)
                model.rotation = Quaternion.LookRotation(forward);

        }
    }

    void FoxUpdate()
    {

        if (Input.GetAxis("Axis-3") != 0 || Input.GetAxis("Axis-4") != 0 && !stats.playerStatus.isDashing)
        {
            RecalculatePivot(cameraPosition);
        }

        if ((stats.playerStatus.isGrounded || jumpCount < jumpTimes) && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (stats.playerStatus.isMoving || Input.GetAxis("Axis-4") != 0 && !stats.playerStatus.isDashing)
        {
            
            model.rotation = Quaternion.LookRotation(joystickDirection);

        }
    }

    void Move(Vector3 forward, Vector3 right, Vector3 dir)
    {

        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {

            dir = (Input.GetAxisRaw("Horizontal") * right * velocity) +
                (Input.GetAxisRaw("Vertical") * forward * velocity);
            velocity += accelRatePerSec * Time.deltaTime;
            velocity = Mathf.Min(velocity, speed);

            stats.playerStatus.isMoving = true;
            anim.SetBool("isMoving", stats.playerStatus.isMoving);
            
        }
        else if (velocity != 0)
        {
            velocity = 0;
            /*velocity -= decelRatePerSec * Time.deltaTime;
            velocity = Mathf.Max(velocity, 0);*/

        }
        else if (velocity == 0)
        {
            dir = Vector3.zero;
            stats.playerStatus.isMoving = false;
            anim.SetBool("isMoving", stats.playerStatus.isMoving);
        }

        float storeY = _moveDirection.y;
        if (stats.playerStatus.isGrounded)
        {
            CalculateSpeed();
        }
 
        _moveDirection = dir;
        _moveDirection.y = storeY;
        if(Input.GetAxisRaw("Horizontal")!=0|| Input.GetAxisRaw("Vertical") != 0){
            joystickDirection = dir.normalized;
            joystickDirection.y = 0;
        }
        


    }

    void Jump()
    {
        if(!stats.playerStatus.isFox)
            _moveDirection.y = jumpForce;
        else
        {
            _moveDirection.y = jumpFox;
            foxModel.GetComponent<Animator>().SetTrigger("Jump");
        }
            

        stats.playerStatus.isJumping = true;
        jumpCount++;

    }

    void IsGrounded()
    {

            
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

        if(Input.GetAxis("Run") != 0 && !stats.playerStatus.isRunning)
        {
            if (stats.playerStatus.isFox)
                speed = runFoxSpeed;
            else
                speed = runSpeed;
            stats.playerStatus.isRunning = true;
        }
        else if(Input.GetAxis("Run")==0 && stats.playerStatus.isRunning)
        {
            speed = walkSpeed;
            stats.playerStatus.isRunning = false;
        }
    }

    public void UpdateHp(float value)
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
        if (percentage == 0)
        {
            stats.playerStatus.isDead = true;
            levelChanger.FadeToLevel(SceneController.Scene.FinalScene);
            stats.win = false;
        }

    }

    void InitStats()
    {
        playerCondition.ChangeCondition(PlayerCondition.Conditions.Main);
        stats.hp.value = stats.hp.maxValue;
        stats.baseDamage = 1;
        stats.ammo = stats.maxAmmo;
        jumpCount = 1;
    }

    void Attack()
    {
        stats.playerStatus.isCharging = true;
        anim.SetBool("isCharging", stats.playerStatus.isCharging);
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
        if (Input.GetButtonDown("Dash") && !stats.playerStatus.isDashing && stats.playerStatus.isGrounded)
        {

            _moveDirection.y = 0;
            dashDirection = _moveDirection;
            currentDashTime = 0.0f;
            if (dashDirection == Vector3.zero)
                dashDirection = forward * -1f * speed;
            stats.playerStatus.isDashing = true;
        }

        if (currentDashTime < maxDashTime && stats.playerStatus.isDashing)
        {
            currentDashTime += dashStoppingSpeed;
            cc.Move(dashDirection * dashSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("StopDash");
            stats.playerStatus.isDashing = false;
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
            Vector3 targetNoY = target.position;
            targetNoY.y = transform.position.y;

            Vector3 dirToTarget = (targetNoY - transform.position).normalized;

            Interact(target, dirToTarget);
        }
    }

    void Interact(Transform target, Vector3 dirToTarget)
    {
        Debug.DrawRay(transform.position, dirToTarget, Color.red, 1f);

        if (target.tag == "Climb" && Vector3.Angle(model.forward, dirToTarget) < 180 && !stats.playerStatus.isFox)
        {

            stats.playerStatus.isClimbing = true;
            stats.playerStatus.isGrounded = false;
            _moveDirection.y = 0;
        }
        else if (Vector3.Angle(model.forward, dirToTarget) < viewAngle / 2)
        {

            float dstToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
            {
                target.GetComponent<IInteractObjects>().OnInteract();
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            stats.playerStatus.isClimbing = false;
        }

        if(other.tag == "ClimbBoundaries")
        {
            stats.playerStatus.isClimbing = false;
        }
        if(other.tag == "HitBoxEnemy" && !GodMode.Instance.isGodMode)
        {
            UpdateHp(-10);
            vfx.PlayVFX(AudioLibrary.VfxSounds.SwordHit);
           
        }
        if (other.tag == "LamiaAttack" && !GodMode.Instance.isGodMode)
        {
            UpdateHp(-7);
        }



    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BossAttack" && !GodMode.Instance.isGodMode)
        {
            UpdateHp(-1f);
        }
    }

    void Climb()
    {
        

        RaycastHit hit;
        Debug.DrawRay(transform.position, forward,Color.blue, 30f);
        Debug.Log(Physics.Raycast(transform.position, forward, out hit,300f, climbMask));

        
        if(Physics.Raycast(transform.position, forward, out hit, 5f, climbMask)) {
            stats.playerStatus.isClimbing = true;
            Debug.Log(hit.transform.gameObject.layer);

            Vector3 climbDirection;

            climbDirection = new Vector3(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

            cc.Move(climbDirection * speed * Time.deltaTime);

        }
        else
        {
            stats.playerStatus.isClimbing = false;
        }

        
    }

    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, forward, out hit, 5f, climbMask);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, forward);
        
    }

    private void Transform()
    {
        playerCondition.ChangeCondition(PlayerCondition.Conditions.Fox);
        stats.playerStatus.isFox = true;
        jumpTimes = 2;
        model.gameObject.SetActive(false);
        model = foxModel;
        model.gameObject.SetActive(true);
    }
    private void UnTransform()
    {
        playerCondition.ChangeCondition(PlayerCondition.Conditions.Main);
        stats.playerStatus.isFox = false;
        jumpTimes = 1;
        model.gameObject.SetActive(false);
        model = playerModel;
        model.gameObject.SetActive(true);

    }

    public void GoTo(Transform pos)
    {
        cc.enabled = false;
        transform.position = pos.position;
        cc.enabled = true;
    }
    public void SwitchModel(Transform mts)
    {
        model.gameObject.SetActive(false);
        model = mts;
        model.gameObject.SetActive(true);
    }

    public void SetGodMode(bool isGod)
    {
        if (isGod)
        {

            jumpTimes = 999;
            speed = 25;
        }
        else
        {

            jumpTimes = 1;
            speed = 4;
        }
    }
}
