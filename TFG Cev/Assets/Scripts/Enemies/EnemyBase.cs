using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    float hp;

    [SerializeField]
    float speed;
    [SerializeField]
    float timeWaiting;
    [SerializeField]
    float detectionRadius;
    [SerializeField]
    float escapeRadius;
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    NavMeshAgent agent;

    float currentTime;

    Vector3 _moveDirection;

    Transform currentNode;

    Transform target;

    bool isWaiting = false;
    bool isPatroling = false;
    bool isChasing = false;

    [SerializeField]
    NodeManager nm;



    //--------Test---------//
    [SerializeField]
    Text hpText;

    [SerializeField]
    Renderer mat;

    private void Start()
    {
        currentNode = nm.GetNode(0);
        isPatroling = true;
        _moveDirection = Vector3.forward;
    }

    private void Update()
    {

        PatrolBehave();
        if (!isChasing)
            FindTargets();
        else
            Chase();

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerHitBox")
        {

            GetDamage(GameObject.Find("Player").GetComponent<PlayerController>().damageDealt);

            mat.material.SetColor("_Color",Random.ColorHSV());
        }

        if(other.tag == "Node")
        {
            isPatroling = false;
            isWaiting = true;
            currentNode = nm.NextNode(currentNode);
        }
    }

    void GetDamage(float dmg)
    {
        hp -= dmg;

        hpText.text = hp.ToString();
    }
    
    

    void PatrolBehave()
    {
        if (isPatroling)
        {
            agent.SetDestination(currentNode.position);
            transform.LookAt(currentNode.position);
        }else if (isWaiting)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeWaiting)
            {
                currentTime = 0;
                isWaiting = false;
                isPatroling = true;


            }
        }
    }

    private void FindTargets()
    {

        Collider[] targets = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        for(int i=0; i < targets.Length; i++)
        {
            if(targets[i]!=null)
            {
                isChasing = true;
                isPatroling = false;
                isWaiting = false;
                target = targets[i].transform;
            }
                
        }
    }
    private void MantainChase()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, escapeRadius, playerLayer);


        if (targets.Length == 0)
        {
            isChasing = false;
            isPatroling = true;
            target = null;

        }

    
        
    }

    void Chase()
    {
        agent.SetDestination(target.position);
        transform.LookAt(target);
        MantainChase();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, escapeRadius);
    }



}
