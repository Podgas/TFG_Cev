using System.Collections;
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
        Waiting,
        ChooseBehave,
        MeleeAttack,
        RangeAttack,
        GettingDamage,
        ChangingPhase,
        Dead
    }

    private static Phases currentPhase = Phases.Phase1;
    private BossState currentState = BossState.Waiting;
    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    float speed;

    [Header("Wait Variables")]
    [SerializeField]
    float coolDown;

    float currentCDTime;

    [Header("Combat Variables")]
    [SerializeField]
    float meleeRange;
    [SerializeField]
    float behaveRange;


    float distance;
    GameObject target;

    [SerializeField]
    ParticleSystem rangeAttack;


    private void Awake()
    {
        target = GameObject.Find("Player");
    }

    void Update()
    {
        //Calculate distance between boss and player + Rotation
        transform.LookAt(target.transform);
        DistanceToTarget();

        switch (currentState)
        {

            case BossState.Waiting:
                WaitForAttack();
                break;
            case BossState.ChooseBehave:
                currentState = ChooseBehave();
                break;
            case BossState.MeleeAttack:
                MeleeBehave();
                break;
            case BossState.RangeAttack:
                RangeBehave();
                RangeAttack();
                currentState = BossState.Waiting;
                break;
            case BossState.GettingDamage:
                break;
            case BossState.ChangingPhase:
                break;

        }

    }


    private void WaitForAttack()
    {
        Debug.Log("Waiting");
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
        if(distance <= meleeRange)
        {

        }
        else
        {
            GoToPlayer();
        }
    }
    private void RangeBehave()
    {
        RangeAttack();
    }

    private void GoToPlayer()
    {
        agent.SetDestination(target.transform.position);
    }

    private void DistanceToTarget()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
    }
    
    private void RangeAttack()
    {
        ParticleSystem ps = Instantiate(rangeAttack, transform.position, Quaternion.identity);
        ps.transform.LookAt(target.transform);
    }
}
