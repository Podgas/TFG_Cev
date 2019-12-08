using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class OrochiBehave : EnemyBase
{

    [SerializeField]
    float attackCD;

    float combatTime;

    [Task]
    protected bool isWaitingCombat = true;
    [Task]
    protected bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
    }

    [Task]
    protected void CombatWait()
    {
        combatTime += Time.deltaTime;
        if (combatTime >= attackCD)
        {
            Debug.Log("Waiting");
            combatTime = 0;
            isAttacking = true;
            isWaitingCombat = false;
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
        
    } 

    [Task]
    protected void HorizontalAttack()
    {
        
        Debug.Log("HorizontalAttack");
        isAttacking = false;
        isWaitingCombat = true;
        anim.SetTrigger("horizontalAttack");
    }

    [Task]
    protected void VerticalAttack()
    {
        Debug.Log("VerticalAttack");
        isAttacking = false;
        isWaitingCombat = true;
        anim.SetTrigger("verticalAttack");
    }

    public void ActivateHitCollider()
    {
        hitCollider.SetActive(!hitCollider.activeSelf);
    }
    public void DesactivateHitCollider()
    {
        hitCollider.SetActive(!hitCollider.activeSelf);
    }
    

}
