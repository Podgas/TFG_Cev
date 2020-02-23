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
    [SerializeField]
    VoidEvent onPlayerDetect;

    float combatTime;

    [Task]
    protected bool isWaitingCombat = true;
    [Task]
    protected bool isAttacking = false;

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
            PlayerDetect();
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }

    private void PlayerDetect()
    {
        Time.timeScale = 0.05f;
        onPlayerDetect.Raise();
    }


}
