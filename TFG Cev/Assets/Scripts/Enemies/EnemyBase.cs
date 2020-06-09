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
    protected LayerMask playerLayer;
    [SerializeField]
    protected FieldOfViewSystem enemyFov;
    [SerializeField]
    Animator exclamation;
    [SerializeField]
    float rotationTime;

    Transform _currentNode;
    protected Transform _target;

    protected Target _currentTarget;
    Vector3 _moveDirection;

    [Header("Components")]
    [SerializeField]
    protected NavMeshAgent agent;
    [SerializeField]
    NodeManager nm;
    [SerializeField]
    protected GameObject hitCollider;


    float currentTime;

    [Header("Animation")]
    [SerializeField]
    protected Animator anim;


    protected Vector3 alertPosition;

    protected bool isWaiting = false;
    protected bool isPatroling;
    protected bool isAlert = false;
    protected bool isCombat = false;
    protected bool isChasing = false;
    protected bool isAlive = true;

    //ENUMERATORS STATE MACHINES
    [SerializeField]
    protected EnemyBehaviour behaviourType;

    protected EnemyBaseStates currentBaseState;
    [SerializeField]
    protected PatrolStates currentPatrolState;

    protected virtual void Start()
    {
        if (nm != null)
            _currentNode = nm.GetNode(1);
        else
            Debug.LogWarning("Pathfinder not assigned");

        if(behaviourType != EnemyBehaviour.Static)
            currentBaseState = EnemyBaseStates.Patrol;
        else
            currentBaseState = EnemyBaseStates.Static;

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
                            _currentNode = nm.NextNode(_currentNode);
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

    protected virtual void CombatUpdate() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitBox")
        {
            GetDamage(GameObject.Find("Player").GetComponent<PlayerController>().damageDealt);
            /*vfx.PlayVFX(AudioLibrary.VfxSounds.SwordHit);
            vfx.PlayVFX(AudioLibrary.VfxSounds.Hurt);*/

        }
        Debug.Log(gameObject.name + " - Node");
        if (other.tag == "Node" && other.transform.parent.name == nm.name)
        {
            
            _currentNode = nm.NextNode(_currentNode);
            currentPatrolState = PatrolStates.Wait;
        }
    }

    void GetDamage(float dmg)
    {
        hp -= dmg;
    }

    protected void Wait()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeWaiting)
        {
            currentTime = 0;
            currentPatrolState = PatrolStates.FindNode;
        }
    }

    protected void Alert()
    {
        agent.isStopped = true;
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
        Debug.Log("Mirando");
        StartCoroutine("SmoothRotation",lookPoint);
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
        currentPatrolState = PatrolStates.Alerted;
        exclamation.SetTrigger("Alert");

    }
    public void OnDetect(Target target)
    {
        _currentTarget = target;
        agent.speed = chaseSpeed;
        currentPatrolState = PatrolStates.Chase;

    }
    public void OnCalm(Target target)
    {
        _currentTarget = target;
        StartCoroutine("RestartPatroling");
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
            for(int i = 0; i < targetsForCombat.Length; i++)
            {
                currentBaseState = EnemyBaseStates.Combat;
            }
        }
    }

    protected enum EnemyBehaviour
    {
        PatrolCatch,
        PatrolFight,
        InPlace,
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

}
