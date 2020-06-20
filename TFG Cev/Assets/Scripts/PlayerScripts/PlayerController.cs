using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject damageVolume;
    struct ControllerVars
    {
        public Vector3 center;
        public float height;
        public float radius;
    }

    //------------Components Vars ---------------//
    [Header("Components")]
    [SerializeField]
    Transform cameraPosition;
    [SerializeField]
    Transform aimCameraPosition;
    [SerializeField]
    PlayerCondition playerCondition;
    [SerializeField]
    public Transform playerModel;
    [SerializeField]
    private Transform foxModel;

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
    private CharacterController cc;

    private float speed;
    private Vector3 _moveDirection;
    //Var rotacion player
    Vector3 joystickDirection;
    public Vector3 forward;
    Vector3 right;
    //Vars aceleracion/deceleracion
    float accelRatePerSec;
    float decelRatePerSec;
    float velocity;

    ControllerVars foxCC;
    ControllerVars humanCC;

    //------------Jump Vars ---------------//
    [Header("Jumping")]
    [SerializeField]
    float gravity = 2.5f;
    [SerializeField]
    float fallMultiplier;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpFox;
    [SerializeField]
    public int jumpTimes;

    int jumpCount = 0;
    float jumpCoolDown = 0;

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

    //--------Interact Vars-------//
    Transform objectToInteract;

    //--------Climb Vars----------//
    [SerializeField]
    private LayerMask climbMask;
    //TODO: climbSpeed

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


    //-----------Sound Vars--------------//
    [Header("SoundVars")]
    [SerializeField]
    AudioLibrary vfx;

    //Variables para ejecutar el sonido de pasos
    float actualFootTime;
    float footTime = 0.5f;

    //-----------Other Vars---------------/
    [Header("OtherVars")]
    [SerializeField]
    LevelChanger levelChanger;



    [SerializeField]
    Transform spawnPoint;

    public bool attak1;

    bool inWagon;
    private void Awake()
    {
        InitStats();
        stats.canMove = true;
    }

    void Start()
    {
        //TODO: Controlar por dispositivo utilizado
        Cursor.visible = false;

        if(stats.spawnPoint == Vector3.zero)
        {
            stats.spawnPoint = spawnPoint.position;
        }
        transform.position = stats.spawnPoint;
        RecalculatePivot(cameraPosition);
        
    }

    void Update()
    {
        if (stats.canMove) {
            //Comprobamos que el juego no esta pausado ni estamos muertos
            if (!MenuManager.GetPaused() || !stats.playerStatus.isDead)
            {
                jumpCoolDown += Time.deltaTime;
                if (stats.playerStatus.isGrounded && jumpCoolDown >= 0.5f)
                {
                    jumpCount = 0;
                    jumpCoolDown = 0;
                }

                //Comprobación climb
                if (stats.playerStatus.isClimbing)
                {
                    Climb();
                }
                else
                {
                    //TODO: Realizar isGrounded propio
                    stats.playerStatus.isGrounded = cc.isGrounded;
                    anim.SetBool("grounded", cc.isGrounded);
                    if (Input.GetButtonDown("Transform"))
                    {
                        Metamorphosis();
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

                    //Comprobamos si el jugador toca el joystick
                    if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
                    {
                        anim.SetBool("move", true);
                        Move(forward, right);
                    }
                    else
                    {
                        anim.SetBool("move", false);
                        Brake();
                    }

                    //Aplicamos la fuerza dela gravedad en funcion de si saltamos o caemos.
                    if (!stats.playerStatus.isGrounded)
                    {
                        if (stats.playerStatus.jumpPressed)
                            _moveDirection.y += Physics.gravity.y * gravity * Time.deltaTime;
                        else
                            _moveDirection.y += Physics.gravity.y * fallMultiplier * Time.deltaTime;
                    }
                    else
                    {
                        //TODO: realizar reproducción de sonidos
                        actualFootTime += Time.deltaTime;
                        if (actualFootTime >= footTime)
                            actualFootTime = 0;
                    }

                    //Movemos al jugador
                    cc.Move(_moveDirection * Time.deltaTime);

                    //Sistema de interacción
                    if (stats.playerStatus.interactPressed)
                    {
                        if (objectToInteract != null)
                        {
                            Interact(objectToInteract);
                        }
                        if (inWagon)
                        {
                            SceneController.LoadScene(SceneController.Scene.Fortess, true);
                            stats.spawnPoint = Vector3.zero;
                        }
                        stats.playerStatus.interactPressed = false;
                    }
                }
            }
        }
        
        
    }
    //Se ejecuta cuando alguno de los otors estados no esta activo
    void MainUpdate()
    {
    
        if (stats.playerStatus.isMoving && !stats.playerStatus.isDashing)
        {
            RecalculatePivot(cameraPosition);
        }

        if (stats.playerStatus.jumpPressed)
        {
            
            stats.playerStatus.jumpPressed = false;

            //Debug.Log((stats.playerStatus.isGrounded || jumpCount < jumpTimes) && stats.playerStatus.jumpPressed);
            if (jumpCoolDown >= 0.5f && stats.playerStatus.isGrounded || (jumpCount < jumpTimes))
            {
                anim.SetTrigger("jump");
                jumpCoolDown = 0;
                Jump();
            }
        }
        

        //TODO: Pulir carga de ataque
        if (Input.GetButtonDown("Attack"))
        {

            if (!attak1)
            {
                anim.SetTrigger("attk1");
                attak1 = true;
            }
            else if (attak1)
            {
                anim.SetTrigger("attk2");
                attak1 = false;
            }

            Attack();
        }

        if (stats.playerStatus.isDashing || Input.GetButtonDown("Dash"))
        {
            Dash();
        }
        #region Debug
        /*
        Debug.DrawRay(transform.position, forward, Color.blue);
        Debug.DrawRay(transform.position, right, Color.green);
        Debug.DrawRay(transform.position, transform.up, Color.red);*/
        #endregion
        //Gestionamos la rotación del modelo

        if (stats.playerStatus.isMoving && 
                ((Input.GetAxisRaw("Horizontal") >= 0.1 || Input.GetAxisRaw("Vertical") >= 0.1) ||
                (Input.GetAxisRaw("Horizontal") <= -0.1 || Input.GetAxisRaw("Vertical") <= -0.1)))
        {
            if (!stats.playerStatus.isDashing)
            {
                //TODO: Agregar velocidad de rotación
                model.rotation = Quaternion.Lerp(model.rotation, Quaternion.LookRotation(joystickDirection *
                                    Time.deltaTime), 10 * Time.deltaTime);
            }       
        }
    }
    //Se ejecuta cuando estamos apuntando con el arma
    void AimUpdate()
    {
        if (Input.GetAxis("Axis-4") != 0)
        {
            RecalculatePivot(aimCameraPosition);
        }
        if (stats.playerStatus.isMoving ||
            (playerCondition.GetCondition() == PlayerCondition.Conditions.Aim && Input.GetAxis("Axis-4") != 0))
        {
            if (!stats.playerStatus.isDashing)
                model.rotation = Quaternion.LookRotation(forward);
        }
    }

    //Mueve al player en función del pivote calculado
    //TODO: Unificar el pivote en una unica variable
    //TODO: Modificar el movimiento aereo
    void Move(Vector3 forward, Vector3 right)
    {
        //TODO: Revisar el cauclo de velocidad
        if (stats.playerStatus.isGrounded)
        {
            CalculateSpeed();
        }
        //Aceleramos al movernos y controlamos que el máximo nunca sea mas que la velocidad máxima
        velocity += accelRatePerSec * Time.deltaTime;
        velocity = Mathf.Min(velocity, speed);

        Vector3 dir;
        dir = (Input.GetAxisRaw("Horizontal") * right * velocity) +
            (Input.GetAxisRaw("Vertical") * forward * velocity);
        stats.playerStatus.isMoving = true;
        //anim.SetBool("isMoving", stats.playerStatus.isMoving);
        //Guardamos la dirección en Y en caso de estar saltando
        float storeY = _moveDirection.y;
        _moveDirection = dir;
        _moveDirection.y = storeY;

        if(Input.GetAxisRaw("Horizontal")!=0|| Input.GetAxisRaw("Vertical") != 0){
            joystickDirection = dir.normalized;
            joystickDirection.y = 0;
        }
    }
    //Frenamos al jugador
    void Brake()
    {
        Vector3 _dir = Vector3.zero;
        velocity -= decelRatePerSec * Time.deltaTime;
        velocity = Mathf.Max(velocity, 0f);
        if (velocity == 0)
        {
            stats.playerStatus.isMoving = false;
            //anim.SetBool("isMoving", stats.playerStatus.isMoving);
        }
        float storeY = _moveDirection.y;
        _moveDirection = _dir;
        _moveDirection.y = storeY;
    }
    //Saltamos en función de la forma del jugador
    void Jump()
    {
        
        if(!stats.playerStatus.isFox)
            _moveDirection.y = jumpForce;
        else
        {
            _moveDirection.y = jumpFox;
            foxModel.GetComponent<Animator>().SetTrigger("Jump");
        }
        jumpCount++;

    }

    //Recalcula el pivote de referencia para las rotaciones
    void RecalculatePivot(Transform camPosition)
    {
        forward = transform.position - camPosition.position;
        right = -Vector3.Cross(forward.normalized, transform.up.normalized);
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
    }
    //Asignamos a la velocidad su valor en función de la forma y si estamos apretandoe l botón de correr.
    void CalculateSpeed()
    {
        if(Input.GetAxis("Run") != 0 && !stats.playerStatus.isRunning)
        {
            if (stats.playerStatus.isFox)
                speed = runFoxSpeed;
            else
                speed = runSpeed;
            stats.playerStatus.isRunning = true;
            anim.SetBool("run", true);
        }
        else if(Input.GetAxis("Run")==0 && stats.playerStatus.isRunning)
        {
            speed = walkSpeed;
            stats.playerStatus.isRunning = false;
            anim.SetBool("run", false);
        }
    }
    //Llamamos a esta función cuando recibimos daño o cura
    public void UpdateHp(float value)
    {
        stats.hp.value += value;
        //calculamos el nuevo porciento de vida
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
        //Lanzamos el evento capturado por la UI
        onHPChange.Raise(percentage);
        //Perdemos
        if (percentage == 0)
        {
            stats.playerStatus.isDead = true;
            levelChanger.FadeToLevel(SceneController.Scene.FinalScene);
            stats.win = false;
        }
    }

    //Inicializamos estadisticas y estados del player
    void InitStats()
    {

        humanCC.height = cc.height;
        humanCC.radius = cc.radius;
        humanCC.center = cc.center;

        Vector3 center = Vector3.zero;
        center.y = 0.42f;
        center.z = 0.06f;

        foxCC.height = 0.64f;
        foxCC.radius = 0.47f;
        foxCC.center = center;

        playerCondition.ChangeCondition(PlayerCondition.Conditions.Main);

        model = playerModel;

        stats.hp.value = stats.hp.maxValue;
        stats.baseDamage = 1;
        stats.ammo = stats.maxAmmo;

        stats.InitStatus();

        speed = walkSpeed;
        accelRatePerSec = speed / timeToMax;
        decelRatePerSec = speed / timeToMin;
    }

    void Attack()
    {
        anim.SetBool("haveSword", true);


    }
    //TODO: Rehacer dash
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
            stats.playerStatus.isDashing = false;
        }
    }

    void Interact(Transform objectToInteract)
    {
        if (!stats.playerStatus.isFox) {

            switch (objectToInteract.tag)
            {
                case "Climb":
                    stats.playerStatus.isClimbing = true;
                    stats.playerStatus.isGrounded = false;
                    _moveDirection.y = 0;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Climb")
        {
            stats.playerStatus.canClimb = true;
            objectToInteract = other.transform;
        }
        //TODO:Revisar funcionamiento
        //TODO: Reacer para recibir daño por cada enemigo en función del ataque
        if(other.tag == "HitBoxEnemy" && !GodMode.Instance.isGodMode)
        {
            UpdateHp(-25);

            //vfx.PlayVFX(AudioLibrary.VfxSounds.SwordHit);
        }

        if(other.tag == "Wagon")
        {
            inWagon = true;
        }
        if (other.tag == "Checkpoint")
        {
            stats.spawnPoint = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Climb")
        {
            stats.playerStatus.canClimb = false;
            objectToInteract = null;
        }
        if (other.tag == "Wagon")
        {
            inWagon = false;
        }
    }

    //Fncion apra controlar la escalada
    void Climb()
    {
        if (stats.playerStatus.canClimb)
        {
            //Movemos en X,Y para trepar (pared) 
            Vector3 climbDirection;
            climbDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

            //TODO: Unificar los move
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
    //Transforma al jugador en Zorro
    private void Transform()
    {
        stats.playerStatus.isFox = true;
        jumpTimes = 2;
        model.gameObject.SetActive(false);
        model = foxModel;
        model.gameObject.SetActive(true);
        SetCC(foxCC);
    }
    //Transforma el jugador en Humanoide
    private void UnTransform()
    {
        playerCondition.ChangeCondition(PlayerCondition.Conditions.Main);
        stats.playerStatus.isFox = false;
        jumpTimes = 1;
        model.gameObject.SetActive(false);
        model = playerModel;
        model.gameObject.SetActive(true);
        SetCC(humanCC);
    }
    //Controlamos el cambio de forma
    private void Metamorphosis() {
        //TODO: Unificar cambio de forma en esta función
        if (stats.playerStatus.isFox)
            UnTransform();

        else Transform();
    }
    //TODO: Rehacer GODMODE
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

    private void SetCC(ControllerVars cv)
    {
        cc.height = cv.height;
        cc.center = cv.center;
        cc.radius = cv.radius;
    }

    


}
