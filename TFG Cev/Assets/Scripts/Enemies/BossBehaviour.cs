using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{

    private enum Phases
    {
        Phase0,
        Phase1,
        Phase2,
        Phase3
    }
    private static Phases currentPhase = Phases.Phase0;

    [SerializeField]
    GameObject lamiaPrefab;

    [SerializeField]
    private List<Transform> lamiaSpawnPoints = new List<Transform>();
    int currentLamias = 0;

    
    int hits;
    [SerializeField]
    int maxHits;
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    float speed;

    [SerializeField]
    float castTime;

    float currentCastTime;

    float delay = 1f;
    float currentTime;
    [SerializeField]
    float knockBackForce;

    public bool isPushing =false;
    public bool isKnockBack = false;
    public bool phaseChange = false;

    float knockbackTime=0.2f;
    float knockBackActualTime;

    [SerializeField]
    GameObject earthQuakePrefab;
    [SerializeField]
    LevelChanger levelChanger;
    [SerializeField]
    PlayerStats stats;

    void Update()
    {
        currentTime += Time.deltaTime;
        switch (currentPhase)
        {
            case Phases.Phase0:

                break;
            case Phases.Phase1:
                if(currentLamias< 2)
                    SpawnLamia(currentLamias);
                    Cast();
                break;

            case Phases.Phase2:
                if (currentLamias < 4)
                    SpawnLamia(currentLamias);
                    Cast();

                break;

            case Phases.Phase3:
                if (currentLamias < 6)
                    SpawnLamia(currentLamias);
                    Cast();
                break;

        }
        if (phaseChange)
            PushPlayer();
        if (isKnockBack)
        {
            knockBackActualTime += Time.deltaTime;
            if (knockbackTime <= knockBackActualTime)
            {
                isPushing = false;
                isKnockBack = false;
                knockBackActualTime = 0;
                //player.GetComponent<PlayerController>().dir = Vector3.zero;
            }

        }

    }

    public void ChangePhase()
    {
        currentPhase++;
        castTime -= 1f;
        foreach(GameObject attacks in GameObject.FindGameObjectsWithTag("BossAttack"))
        {
            Destroy(attacks);
        }
        foreach (GameObject attacks in GameObject.FindGameObjectsWithTag("LamiaAttack"))
        {
            Destroy(attacks);
        }
    }

    private void SpawnLamia(int index)
    {
        Instantiate(lamiaPrefab,lamiaSpawnPoints[index].position,Quaternion.identity);
        currentLamias++;
    }

    private void OnPlayerHit()
    {
        Debug.Log("TE EMPUJE");
        isPushing = true;
        hits++;
        ChangePhase();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerHitBox")
        {
            if (currentTime >= delay)
            {
                if (hits == maxHits-1) {
                    phaseChange = true;
                    ChangePhase();
                    stats.win = true;
                    levelChanger.FadeToLevel(SceneController.Scene.FinalScene);
                    
                }
                else
                {
                    OnPlayerHit();
                    currentTime = 0;
                    phaseChange = true;
                }
                
                
            }
            
        }
    }

    void PushPlayer()
    {

        player.position = Vector3.MoveTowards(player.position, startPoint.position, speed * Time.deltaTime);
        if(player.position == startPoint.position)
        {
            isPushing = false;
            phaseChange = false;
        }
    }

    void Cast()
    {
        currentCastTime += Time.deltaTime;

        if (currentCastTime >= castTime)
        {
            
            EarthQuake();
           
            currentCastTime = 0;
        }
    }

    void KnockBack()
    {
        Vector3 direction =  player.position - transform.position;
        direction = direction.normalized;
        direction.y = 0;

        isKnockBack = true;
        isPushing = true;
        //player.GetComponent<PlayerController>().dir = direction * knockBackForce;
    }
    void EarthQuake()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0;
        Instantiate(earthQuakePrefab, spawnPos, Quaternion.identity);
    }
}
