using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyBase : MonoBehaviour
{


    [Header("Enemy Stats")]
    [SerializeField]
    float hp;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float chaseSpeed;

    [Header("Patrol Vars")]
    [SerializeField]
    float timeWaiting;
    [SerializeField]
    float escapeRadius;
    [SerializeField]
    protected float combatRadius;
    [SerializeField]
    float rotationTime;
    [SerializeField]
    Transform _currentNode;

    [Header("FieldOfView Vars")]
    [SerializeField]
    protected LayerMask playerLayer;
    [SerializeField]
    protected FieldOfViewSystem enemyFov;
    [SerializeField]
    Animator exclamation;

    [Header("Components")]
    [SerializeField]
    protected NavMeshAgent agent;
    [SerializeField]
    NodeManager nm;
    [SerializeField]
    protected GameObject hitCollider;

    [Header("Animation")]
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    float chaseTimer = 0;
    [SerializeField]
    AnimationClip detectAnim;

    //ENUMERATORS STATE MACHINES
    [Header("Info Vars")]
    [SerializeField]
    protected EnemyBehaviour behaviourType;
    [SerializeField]
    protected EnemyBaseStates currentBaseState;
    [SerializeField]
    protected PatrolStates currentPatrolState;

    Vector3 _moveDirection;
    protected Transform _target;
    protected Target _currentTarget;


    float currentTime;
    protected Vector3 alertPosition;
    protected bool isAlive = true;

    [SerializeField]
    VoidEvent onCatch;

    protected virtual void Start()
    {
        if (nm != null)
            _currentNode = nm.GetNode(1);
        else
            Debug.LogWarning("Pathfinder not assigned");

        if(behaviourType != EnemyBehaviour.Static)
            currentBaseState = EnemyBaseStates.Patrol;
        else
        {
            currentBaseState = EnemyBaseStates.Static;
        }
            

        _moveDirection = Vector3.forward;
        agent.speed = speed;
    }

    private void Update()
    {
        //State Machine para controlar el el primel nivel de jerarquia del estado
        switch (currentBaseState)
        {
            case EnemyBaseStates.Static:
                break;
            case EnemyBaseStates.Patrol:

                switch (currentPatrolState)
                {
                    case PatrolStates.FindNode:

                        if (_currentNode == null)
                            _currentNode = nm.GetNode(0);
                        else
                        {
                            currentPatrolState = PatrolStates.GoToNode;
                            if(behaviourType == EnemyBehaviour.InPlace)
                                _currentNode = nm.NextNode(_currentNode);
                            else
                                anim.SetTrigger("move");
                        }
                            
                        break;

                    case PatrolStates.GoToNode:
                        
                        if(behaviourType == EnemyBehaviour.InPlace)
                        {
                            currentPatrolState = PatrolStates.Wait;
                            _currentNode = nm.NextNode(_currentNode);

                            LookToPoint(_currentNode.position);
                        }
                        else
                        {
                            MoveAgent(_currentNode);
                        }

                        break;

                    case PatrolStates.Wait:

                        Wait();
                        break;

                    case PatrolStates.Alerted:
                        Alert();

                        break;
                    case PatrolStates.Chase:

                        if(chaseTimer > detectAnim.length)
                        {
                            Move();
                        }
                        else
                        {
                            chaseTimer += Time.deltaTime;
                        }

                        MoveAgent(_currentTarget.currentPosition);
                        SearchForCombat();
                        break;
                }

                break;

            case EnemyBaseStates.Combat:

                CombatUpdate();

                break;
        }
    }

    protected virtual void CombatUpdate() {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PlayerHitBox")
        {
            GetDamage(GameObject.Find("Player").GetComponent<PlayerController>().damageDealt);
            /*vfx.PlayVFX(AudioLibrary.VfxSounds.SwordHit);
            vfx.PlayVFX(AudioLibrary.VfxSounds.Hurt);*/

        }

        if (currentPatrolState != PatrolStates.Chase && ( other.tag == "Node" && other.transform.parent.name == nm.name) )
        {

            LookToPoint(_currentNode.position + (_currentNode.forward * 2));
            currentPatrolState = PatrolStates.Wait;

        }
    }

    void GetDamage(float dmg)
    {
        hp -= dmg;
    }

    protected void Wait()
    {
        if (currentBaseState != EnemyBaseStates.Static)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeWaiting)
            {
                currentTime = 0;
                currentPatrolState = PatrolStates.FindNode;
            }
        }
    }

    protected void Alert()
    {
        Stop();
    }

    protected void MoveAgent(Transform destination)
    {
        agent.SetDestination(destination.position);
        Vector3 lookPos = destination.position - transform.position;
        lookPos.y = 0;
        Debug.DrawLine(transform.position, destination.position,Color.green);
        Quaternion targetRot = Quaternion.LookRotation(lookPos);
        transform.rotation = targetRot;
    }

    protected void LookToPoint(Vector3 lookPoint)
    {
    
        StartCoroutine("SmoothRotation",lookPoint);
        anim.SetTrigger("stop");
        _currentNode = nm.NextNode(_currentNode);
        currentPatrolState = PatrolStates.Wait;
    }

    IEnumerator SmoothRotation(Vector3 lookPos)
    {
        float currentTimeToRotate = 0;
        float rate = rotationTime / rotationTime;
        bool rotating = true;
        Vector3 relativePosition = lookPos - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(relativePosition);

        while (rotating)
        {   
            currentTimeToRotate += Time.deltaTime * rotationTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, currentTimeToRotate);
            if (currentTimeToRotate > 1)
            {
                rotating = false;
            }
            yield return new WaitForEndOfFrame();
        } 
        
    }
    protected void Die()
    {
        //TODO: implementar animacion de morir, efecto de particulas dedesaparicion y coorutina de destrucción.
        Destroy(gameObject);
    }

    public void OnAlert(Target target)
    {
        _currentTarget = target;
        if (behaviourType != EnemyBehaviour.Static)
        {
            _currentTarget = target;
            currentPatrolState = PatrolStates.Alerted;
            Debug.Log("PASE");
            exclamation.SetTrigger("Alert");
            anim.SetTrigger("alert");
        }
    }
    public void OnDetect(Target target)
    {
        _currentTarget = target;
        agent.speed = chaseSpeed;
        if (behaviourType != EnemyBehaviour.Static)
        {
            
            currentPatrolState = PatrolStates.Chase;
            anim.SetTrigger("detected");
            chaseTimer = 0;
        }

    }
    public void OnCalm(Target target)
    {
        if (behaviourType != EnemyBehaviour.Static)
        {
            _currentTarget = target;
            StartCoroutine("RestartPatroling");
            anim.SetTrigger("move");
        }
    }    

    IEnumerator RestartPatroling()
    {
        yield return new WaitForSeconds(1f);

        currentPatrolState = PatrolStates.GoToNode;
        currentTime = 0;
        agent.isStopped = false;

        
    }
    void SearchForCombat()
    {
        Collider[] targetsForCombat = Physics.OverlapSphere(transform.position, combatRadius, playerLayer);

        if (targetsForCombat != null)
        {

            for (int i = 0; i < targetsForCombat.Length; i++)
            {
                if (behaviourType == EnemyBehaviour.InPlaceCatch || behaviourType == EnemyBehaviour.PatrolCatch)
                {
                    OnCatch();
                }
                else
                {
                    currentBaseState = EnemyBaseStates.Combat;
                    anim.SetTrigger("startCombat");
                }
                
            }  
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, combatRadius);
    }

    protected enum EnemyBehaviour
    {
        PatrolCatch,
        PatrolFight,
        InPlace,
        InPlaceCatch,
        Event,
        Static
    }

    protected enum EnemyBaseStates
    {
        Patrol,
        Combat,
        Static
    }

    protected enum PatrolStates
    {
        FindNode,
        Wait,
        GoToNode,
        Alerted,
        Chase,

    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public void Move()
    {
        agent.isStopped = false;
    }

    public void OnActiveEvent()
    {
        Debug.Log("Eafirmativo");
        behaviourType = EnemyBehaviour.PatrolFight;
        currentBaseState = EnemyBaseStates.Patrol;
        _target = _currentTarget.currentPosition;
        OnDetect(_currentTarget);

    }

    void OnCatch()
    {
        onCatch.Raise();
    }
}
