using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class OrochiBehave : EnemyBase
{

    [SerializeField]
    float attackCD;
    [SerializeField]
    float catchRadius;

    float combatTime;

    CombatStates combatStates = CombatStates.CDAction;

    public GameObject loseCanvas;

    protected override void Start()
    {
        base.Start();
    }
    private void LateUpdate()
    {
        Collider[] player = Physics.OverlapSphere(transform.position, catchRadius, playerLayer);
        if(player.Length > 0)
        {
            //PlayerDetect();
        }
    }

    protected override void CombatUpdate()
    {
        base.CombatUpdate();

        switch (combatStates)
        {
            case CombatStates.CDAction:

                CombatWait();
                break;

            case CombatStates.ChangePosition:

                break;
            case CombatStates.HorizontalAttack:

                HorizontalAttack();
                break;

            case CombatStates.VerticalAttack:

                VerticalAttack();
                break;

            case CombatStates.Dodge:

                break;

        }

    }

    protected void CombatWait()
    {
        combatTime += Time.deltaTime;
        if (combatTime >= attackCD)
        {
            combatTime = 0;
            LaunchAttack();
        }
        Debug.Log("WaitForCD");
        
    }

    void LaunchAttack()
    {
        Random.InitState(Time.frameCount);
        int random = Random.Range(0, 1);
        if (random == 1)
            combatStates = CombatStates.HorizontalAttack;
        else
            combatStates = CombatStates.VerticalAttack;
    }

    protected void HorizontalAttack()
    {
        Debug.Log("HorizontalAttack");
        anim.SetTrigger("horizontalAttack");
        combatStates = CombatStates.CDAction;
    }


    protected void VerticalAttack()
    {
        Debug.Log("VerticalAttack");
        anim.SetTrigger("verticalAttack");
        combatStates = CombatStates.CDAction;
    }

    public void ActivateHitCollider()
    {
        hitCollider.SetActive(!hitCollider.activeSelf);
    }
    public void DesactivateHitCollider()
    {
        hitCollider.SetActive(!hitCollider.activeSelf);
    }

    public void OnCombatTutorial()
    {
        _target = GameObject.Find("Player").transform;
        agent.speed = chaseSpeed;
        currentPatrolState = PatrolStates.Chase;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }

    protected enum CombatStates
    {
        CDAction,
        ChangePosition,
        HorizontalAttack,
        VerticalAttack,
        Dodge,
    }

}
