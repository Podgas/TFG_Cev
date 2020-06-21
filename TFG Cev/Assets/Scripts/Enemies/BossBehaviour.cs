﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour
{

    private enum Phases
    {
        Phase0,
        Phase1,
        Phase2,
        Phase3
    }

    private enum BossState
    {
        Spawn,
        Waiting,
        ChooseBehave,
        MeleeAttack,
        RangeAttack,
        Casting,
        GettingDamage,
        ChangingPhase,
        Dead
    }

    [Header("Boss Stats")]
    [SerializeField]
    int hitsToChangePhase;
    [SerializeField]
    int currentHP;

    [SerializeField]
    private Phases currentPhase = Phases.Phase0;
    [SerializeField]
    private BossState currentState = BossState.Spawn;
    
  

    [Header("Movement")]
    [SerializeField]
    float speed;
    [SerializeField]
    NavMeshAgent agent;

    [Header("Wait Variables")]
    [SerializeField]
    float coolDown;
    float currentCDTime;

    [Header("Combat Variables")]
    [SerializeField]
    float meleeRange;
    [SerializeField]
    float behaveRange;

    [Header("Animation")]
    [SerializeField]
    Animator anim;

    [Header("Skills")]
    [SerializeField]
    ParticleSystem rangeAttack;
    [SerializeField]
    ParticleSystem thunder;
    [SerializeField]
    ParticleSystem clouds;
    [SerializeField]
    ParticleSystem jump;
    [SerializeField]
    ParticleSystem earthQuake;
    [SerializeField]
    Vector2 thunderStrikeCube;
    [SerializeField]
    int thunderNum;
    [SerializeField]
    float thunderDelay;
    float currentThunderTime;
    int thunderCount;
    [SerializeField]
    float attkAngle;
    [SerializeField]
    Vector2 eqPadding;
    [SerializeField]
    Vector2 maxEQRange;
    [SerializeField]
    float eartquakeDelay;
    Vector3 currentEQPosition;
    [SerializeField]
    Transform earthquakeSpawn;

    float distance;
    public GameObject target;

    [Header("Entring Boss")]

    float currentAnimTime;

    [SerializeField]
    Transform spawnPoint1;
    [SerializeField]
    Transform spawnPoint2;
    Transform currentPoint;



    private void Awake()
    {
        target = GameObject.Find("Player");
        currentHP = hitsToChangePhase;
        agent.speed = speed;
        currentPoint = spawnPoint1;

    }

    void Update()
    {
        //Calculate distance between boss and player + Rotation
        
        DistanceToTarget();

        switch (currentState)
        {
            case BossState.Spawn:
                
                break;
            case BossState.Waiting:
                WaitForAttack();
                LookAt();
                break;
            case BossState.ChooseBehave:
                currentState = ChooseBehave();
                break;
            case BossState.MeleeAttack:
                MeleeBehave();
                break;
            case BossState.RangeAttack:
                RangeBehave();
                break;
            case BossState.Casting:
                
                break;
            case BossState.GettingDamage:
                break;
            case BossState.ChangingPhase:
                PhaseChange();
                break;

        }

    }


    private void WaitForAttack()
    {

        if(currentCDTime >= coolDown)
        {
            currentCDTime = 0;
            currentState = BossState.ChooseBehave;
        }
        else
        {
            currentCDTime += Time.deltaTime;
        }
    }

    private BossState ChooseBehave()
    {
        if(distance < behaveRange)
            return BossState.MeleeAttack;

        return BossState.RangeAttack;
    }
    
    private void MeleeBehave()
    {

        if (meleeRange >= distance)
        {
            anim.SetBool("isMoving", false);
            int random = 1;
            if (currentPhase != Phases.Phase1)
            {
                random = Random.Range(0, 100);
            }

            if(random <= 50)
            {
                MeleeAttack();
                currentState = BossState.Casting;
            }
            else
            {
                currentState = BossState.Casting;
                AreaMeleeAttack();
            }
            //TODDO LAUNCH MELEE ATTACK0000000000000000
            
        }
        else
        {
            LookAt();
            anim.SetBool("isMoving", true);
            GoToPlayer();
            ChooseBehave();
        }
    }
    private void RangeBehave()
    {
        int random = 1;

        if(currentPhase == Phases.Phase3)
        {
            random += Random.Range(0, 150);
        }
        else if (currentPhase == Phases.Phase2)
        {
            random += Random.Range(0, 100);
        }

        if (random <= 30)
        {
            currentState = BossState.Casting;
            anim.SetTrigger("rangeAttk");

        }
        else if(random >= 100)
        {
            currentState = BossState.Casting;
            anim.SetTrigger("earthquake"); 
        }
        else
        {
            currentState = BossState.Casting;
            anim.SetTrigger("thunderstorm");
        }


    }

    private void GoToPlayer()
    {
        agent.destination = (target.transform.position);

    }
    private void LookAt()
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(lookPos);
        transform.rotation = targetRot;
    }

    private void DistanceToTarget()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
    }
    
    private void RangeAttack()
    {

        ParticleSystem ps = Instantiate(rangeAttack, transform.position, transform.rotation);
        ParticleSystem ps2 = Instantiate(rangeAttack, transform.position, transform.rotation);
        ParticleSystem ps3 = Instantiate(rangeAttack, transform.position, transform.rotation);
        ps.transform.LookAt(transform.forward * 2);
        ps2.transform.LookAt(transform.forward * 2);
        ps3.transform.LookAt(transform.forward * 2);

        ps2.transform.Rotate(new Vector3(0, 20, 0));
        ps3.transform.Rotate(new Vector3(0, -20, 0));

        
    }
    public void StartThunder()
    {
        StartCoroutine(Thunderstorm());
    }
    IEnumerator Thunderstorm()
    {
        Vector3 spawnPoint = target.transform.position;
        spawnPoint.y += 7;
        Instantiate(clouds, spawnPoint, Quaternion.Euler(new Vector3(-90, 0, 0)));

        for (int i = 0; i < thunderNum; i++)
        {
            SpawnThunder();
            yield return new WaitForSeconds(thunderDelay);
        }    
    }

    public void EarthQuake()
    {
        ParticleSystem go;

        go =Instantiate(earthQuake, earthquakeSpawn.position, earthQuake.gameObject.transform.rotation);
        go.GetComponent<EarthQuakeBehaviour>().SetDirection((target.transform.position - transform.position).normalized);
        currentEQPosition.x = 0;

    }
    private void MeleeAttack()
    {
        anim.SetTrigger("melee");
    }
    private void AreaMeleeAttack()
    {
        anim.SetTrigger("area");
    }

    private void ActivateArea()
    {
        RaycastHit rhit;

        if (Physics.Raycast(earthquakeSpawn.position, Vector3.down, out rhit, 300f, LayerMask.NameToLayer("Ground")))
        {
            Instantiate(earthQuake, rhit.point, earthQuake.gameObject.transform.rotation);
        }

    }

    private void PhaseChange()
    {
        currentHP = hitsToChangePhase;
        switch (currentPhase)
        {
            case Phases.Phase0:
                currentPhase = Phases.Phase1;

                break;

            case Phases.Phase1:
                currentPhase = Phases.Phase2;
                ChangeSpawnPoint();
                break;

            case Phases.Phase2:
                currentPhase = Phases.Phase3;
                ChangeSpawnPoint();
                break;

            case Phases.Phase3:

                break;
        }

        
        currentState = BossState.Waiting;
    }

    public void ChangeSpawnPoint()
    {
        if (currentPoint.position == spawnPoint1.position)
        {
            currentPoint = spawnPoint2;
        }
        else
        {
            currentPoint = spawnPoint1;
        }
        agent.Warp(currentPoint.position);
    }

    private void LandingSpawn()
    {
        currentState = BossState.ChangingPhase;
        anim.SetTrigger("startCombat");
        jump.Play();
    }

    private void SpawnThunder()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-thunderStrikeCube.x, thunderStrikeCube.x), 5, Random.Range(-thunderStrikeCube.y, thunderStrikeCube.y));

        ParticleSystem ps = Instantiate(thunder, target.transform.position + spawnPosition, Quaternion.identity);
        Destroy(ps.gameObject, 3f); 
    }
    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {

        if (!angleIsGlobal)
        {
            angleInDegree += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }
    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireCube(target.transform.position, new Vector3(thunderStrikeCube.x*2, 30,  thunderStrikeCube.y*2));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, behaveRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

    public void GetDamage()
    {
        currentHP--;
        if(currentHP == 0)
        {
            currentState = BossState.ChangingPhase;
        }
        else
        {
            //ANIM DAMAGE
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerHitBox")
        {
            GetDamage();
        }
    }

    public void EndAnim()
    {
        currentState = BossState.Waiting;

        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            if(parameter.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(parameter.name);
            }  
        }
    }
    

}
