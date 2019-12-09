using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class LamiaBehave : EnemyBase
{
    [SerializeField]
    float attackCD;
    [SerializeField]
    float boltCastTime;
    [SerializeField]
    float areaCastTime;
    [SerializeField]
    GameObject boltPrefab;
    [SerializeField]
    GameObject areaPrefab;
    [SerializeField]
    GameObject areaEffectPrefab;

    GameObject areaEffect;

    float combatTime;
    float castTime;

    [Task]
    protected bool isWaitingCombat = true;
    [Task]
    protected bool isAttacking = false;
    [Task]
    protected bool isCasting = false;

    


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
    protected void CastBolt()
    {

        castTime += Time.deltaTime;

        if (castTime >= boltCastTime)
        {
            castTime = 0;
            GameObject projectyle = Instantiate(boltPrefab, hitCollider.transform.position, Quaternion.identity);
            projectyle.GetComponent<ProjectyleBehaviour>().SetDestination(target.position - hitCollider.transform.position);
            Debug.Log("Bolt");
            isAttacking = false;
            isWaitingCombat = true; 
        }

      
    }

    [Task]
    protected void CastArea()
    {
        castTime += Time.deltaTime;
        if (areaEffect == null)
        {
            areaEffect = Instantiate(areaEffectPrefab, target.position, areaEffectPrefab.transform.rotation);
        }
        
        
        if (castTime >= areaCastTime)
        {
            castTime = 0;
            Debug.Log("Area");
            isAttacking = false;
            isWaitingCombat = true;
            StartCoroutine("DestroyArea", areaEffect);
            areaEffect = null;


        }
    }

    [Task]
    protected override void SearchForPlayer()
    {
        base.SearchForPlayer();
        if (isChasing)
        {
            agent.stoppingDistance = combatRadius;
        }else if (isPatroling)
        {
            agent.stoppingDistance = 3;
        }

    }

    public void ActivateHitCollider()
    {
        hitCollider.SetActive(!hitCollider.activeSelf);
    }
    public void DesactivateHitCollider()
    {
        hitCollider.SetActive(!hitCollider.activeSelf);
    }

    IEnumerator DestroyArea(GameObject area)
    {
        yield return new WaitForSeconds(2);
        Destroy(area);
    }

}
