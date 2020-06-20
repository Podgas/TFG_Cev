using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrochiBehave : EnemyBase
{

    [SerializeField]
    float attackCD;
    [SerializeField]
    float attackTimer;
    [SerializeField]
    AnimationClip attk1;
    [SerializeField]
    AnimationClip attk2;
    float durationAnim;

    [SerializeField]
    ParticleSystem attk1ps;

    Vector2 framesGlobal;
    Vector2 frames = new Vector2(42, 50);
    Vector2 frames2 = new Vector2(17,25);
    int frameCounterAnim = 0;
    bool damaging = false;

    float combatTime;

    CombatStates combatStates = CombatStates.CDAction;



    protected override void Start()
    {
        base.Start();
    }
    private void LateUpdate()
    {
        if(currentBaseState == EnemyBaseStates.Combat)
            IsInRange();
        
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

            case CombatStates.StrikeAttack:

                StrikeAttack();
                break;
            case CombatStates.Attacking:
                attackTimer += Time.deltaTime;
                frameCounterAnim++;

                if (attackTimer >= durationAnim)
                {
                    attackTimer = 0;
                    combatStates = CombatStates.CDAction;
                }
                break;
            case CombatStates.Dodge:

                break;
        }
    }


    public void IsInRange()
    {
        Collider[] player = Physics.OverlapSphere(transform.position, combatRadius, playerLayer);
        if (player.Length == 0)
        {
            if(combatStates == CombatStates.CDAction)
            {
                currentBaseState = EnemyBaseStates.Patrol;
                currentPatrolState = PatrolStates.Chase;
            }      
        }
    }
    protected void CombatWait()
    {
        Vector3 lookRot = _currentTarget.currentPosition.position;

        LookToPoint(lookRot);

        combatTime += Time.deltaTime;
        if (combatTime >= attackCD)
        {
            combatTime = 0;
            LaunchAttack();
        }
        
    }

    void LaunchAttack()
    {
        
        Random.InitState(Time.frameCount);
        attackTimer = 0;
        int random = Random.Range(0, 100);
        if (random > 50)
            combatStates = CombatStates.HorizontalAttack;
        else
            combatStates = CombatStates.StrikeAttack;
    }

    protected void HorizontalAttack()
    {

        Debug.Log("Attak1");
        anim.SetTrigger("attak1");
        durationAnim = attk1.length/2;
        combatStates = CombatStates.Attacking;
        framesGlobal = frames;
    }

    public void CastAttk1()
    {
        attk1ps.Play();
        ActivateHitCollider();
    }
    public void CastAttk2()
    {
        ActivateHitCollider();
    }


    protected void StrikeAttack()
    {
        Debug.Log("Attak2");
        anim.SetTrigger("attak2");
        durationAnim = attk2.length;
        combatStates = CombatStates.Attacking;
        framesGlobal = frames2;
    }

    public void ActivateHitCollider()
    {
        hitCollider.SetActive(true);
    }
    public void DesactivateHitCollider()
    {
        hitCollider.SetActive(false);
    }

    protected enum CombatStates
    {
        CDAction,
        ChangePosition,
        HorizontalAttack,
        StrikeAttack,
        Attacking,
        Dodge,
    }



}
