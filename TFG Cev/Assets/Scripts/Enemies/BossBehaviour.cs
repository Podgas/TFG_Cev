using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    float distance;
    GameObject target;

    [Header("Entring Boss")]

    float currentAnimTime;




    private void Awake()
    {
        target = GameObject.Find("Player");
        currentHP = hitsToChangePhase;
    }

    void Update()
    {
        //Calculate distance between boss and player + Rotation
        transform.LookAt(target.transform);
        DistanceToTarget();

        switch (currentState)
        {
            case BossState.Spawn:
                
                break;
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
        

        if (distance <= meleeRange)
        {
            int random = 1;
            if (currentPhase == Phases.Phase2)
            {
                random = Random.Range(0, 100);
            }

            if(random <= 50)
            {
                MeleeAttack();
            }
            else
            {
                AreaMeleeAttack();
            }
            //TODDO LAUNCH MELEE ATTACK0000000000000000
            
        }
        else
        {
            GoToPlayer();
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
            random += Random.Range(0, 50);
        }

        if (random <= 50)
        {

            EarthQuake();
        }
        else if(random >= 100)
        {

            StartCoroutine(Thunderstorm());
        }
        else
        {
            RangeAttack();
        }

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
        Debug.DrawRay(transform.position, Vector3.forward, Color.red, 40f);
        Debug.DrawRay(transform.position, Vector3.back, Color.blue, 40f);

        ParticleSystem ps = Instantiate(rangeAttack, transform.position, transform.rotation);
        ParticleSystem ps2 = Instantiate(rangeAttack, transform.position, transform.rotation);
        ParticleSystem ps3 = Instantiate(rangeAttack, transform.position, transform.rotation);
        ps.transform.LookAt(target.transform);
        ps2.transform.LookAt(target.transform);
        ps3.transform.LookAt(target.transform);

        ps2.transform.Rotate(new Vector3(0, 20, 0));
        ps3.transform.Rotate(new Vector3(0, -20, 0));

        currentState = BossState.Waiting;
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

        currentState = BossState.Waiting;
    }

    public void EarthQuake()
    {
        float storeX;
        Vector3 storepos;
        ParticleSystem go;
        /*while(currentEQPosition.z < maxEQRange.y)
        {*/
        currentEQPosition.x += eqPadding.y;
        storepos = transform.position + currentEQPosition;
        storeX = storepos.x;
        go =Instantiate(earthQuake, storepos, earthQuake.gameObject.transform.rotation);
        go.GetComponent<EarthQuakeBehaviour>().SetDirection((target.transform.position - transform.position).normalized);
        currentState = BossState.Waiting;
        currentEQPosition.x = 0;

    }
    private void MeleeAttack()
    {

    }
    private void AreaMeleeAttack()
    {

    }

    private void PhaseChange()
    {
        currentHP = hitsToChangePhase;
        switch (currentPhase)
        {
            case Phases.Phase0:
                currentPhase = Phases.Phase1;
                Debug.Log("Phase1");
                break;

            case Phases.Phase1:
                currentPhase = Phases.Phase2;
                Debug.Log("Phase2");
                break;

            case Phases.Phase2:
                currentPhase = Phases.Phase3;
                Debug.Log("Phase3");
                break;

            case Phases.Phase3:
                Debug.Log("WON!");
                break;
        }
        currentState = BossState.Waiting;
    }

    private void LandingSpawn()
    {
        currentState = BossState.ChangingPhase;
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
    }

    public void GetDamage()
    {
        currentHP--;
        Debug.Log(currentHP);
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

}
