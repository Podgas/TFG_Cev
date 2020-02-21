using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Panda;


public class EnemyBase : MonoBehaviour
{
    [Header("EnemyStats")]
    [SerializeField]
    float hp;
    [SerializeField]
    float speed;
    [SerializeField]

    [Header("EnemyPatrol")]
    float timeWaiting;
    [SerializeField]
    protected float detectionRadius;
    [SerializeField]
    float escapeRadius;
    [SerializeField]
    protected float combatRadius;
    [SerializeField]
    protected LayerMask playerLayer;
    [SerializeField]
    bool useNode = true;
    

    Transform currentNode;
    Vector3 _moveDirection;

    [Header("Components")]
    [SerializeField]
    protected NavMeshAgent agent;
    [SerializeField]
    NodeManager nm;
    [SerializeField]
    protected GameObject hitCollider;


    float currentTime;

    [SerializeField]
    protected Animator anim;
    
    protected Transform target;
    protected Vector3 alertPosition;


    //------------BT VARS-------//
    [Task]
    protected bool isWaiting = false;
    [Task]
    protected bool isPatroling;
    [Task]
    protected bool isSearching;
    //[Task]
    protected bool isAlert = false;
    [Task]
    protected bool isCombat = false;
    [Task]
    protected bool isChasing = false;
    [Task]
    protected bool isAlive = true;


    /*[SerializeField]
    AudioLibrary vfx;*/

    

    protected virtual void Start()
    {
        if(nm!=null)
            currentNode = nm.GetNode(0);
        isPatroling = true;
        isSearching = true;
        _moveDirection = Vector3.forward;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);
        }
        if (hp <= 0)
        {
            isAlive = false;
        }
        if (isAlert)
        {
            Alert();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerHitBox")
        {

            GetDamage(GameObject.Find("Player").GetComponent<PlayerController>().damageDealt);
            /*vfx.PlayVFX(AudioLibrary.VfxSounds.SwordHit);
            vfx.PlayVFX(AudioLibrary.VfxSounds.Hurt);*/

        }

        if(other.tag == "Node")
        {
            if(other.transform == currentNode)
            {
                isPatroling = false;
                isWaiting = true;
                currentNode = nm.NextNode(currentNode);
            }
                
        }
    }

    void GetDamage(float dmg)
    {
        hp -= dmg;
    }
    
    [Task]
    protected void SearchForCombat()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, combatRadius, playerLayer);

        
        if (targets.Length == 0)
        {
            isCombat = false;
            anim.SetBool("isCombat", isCombat);
            isSearching = true;
            isPatroling = true;
            Task.current.Fail();
        }
        else
        {
            //SetPandaConditionsToFalse();
            isCombat = true;
            anim.SetBool("isCombat", isCombat);
            Task.current.Succeed();

        }
    }
    
    [Task]
    protected void PatrolBehave()
    {
        if (isPatroling && !isAlert && useNode) { 
            agent.SetDestination(currentNode.position);

            Vector3 lookPos = currentNode.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    protected void Wait()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeWaiting)
        {
            currentTime = 0;
            isWaiting = false;
            isPatroling = true;

        }
    }

    [Task]
    protected void Alert()
    {
        if (isAlert)
        {
            agent.isStopped = true;
        }    
    }

    [Task]
    protected void MoveEnemy()
    {
        agent.SetDestination(target.position);
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);

    }
    [Task]
    protected void Chase()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, escapeRadius, playerLayer);
        if (targets.Length == 0)
        {
            target = null;
            SetPandaConditionsToFalse();
            isPatroling = true;
            isSearching = true;
            Task.current.Fail();
        }
        else
        {
            Task.current.Succeed();
        }
    }
    [Task]
    protected void Die()
    {
        Destroy(gameObject);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, escapeRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, combatRadius);
    }
    void SetPandaConditionsToFalse()
    {
        isPatroling = false;
        isSearching = false;
        isWaiting = false;
        isCombat = false;
        isChasing = false;
        isAlert = false;
    }
    public void SetAlert(Vector3 alertDetectionPoint)
    {
        SetPandaConditionsToFalse();
        isAlert = true;
        alertPosition = alertDetectionPoint;
        

    }
    public void SetCalm()
    {
        SetPandaConditionsToFalse();
        isPatroling = true;
        agent.isStopped = false;
    }
    public void OnDetected(GameObject objectDetected)
    {
        SetPandaConditionsToFalse();
        isChasing = true;
        agent.isStopped = false;
        target = objectDetected.transform;
    }


}
