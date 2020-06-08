 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{

    [SerializeField]
    Transform pivot;
    [SerializeField]
    float speed = 15f;
    [SerializeField]
    float drug = 1f;
    [SerializeField]
    float repeatingTime = 1f;
    [SerializeField]
    GameObject craterPrefab;
    [SerializeField]
    float spawnRate = 1f;
    [SerializeField]
    float spawnDuration = 1f;



    float startSpeed = 0f;
    float spawnDur;
    Vector3 stepPosition;


    private void Start()
    {
        InvokeRepeating("StartAgain", 0f, repeatingTime);
        startSpeed = speed;
        stepPosition = pivot.position;
        spawnDur = spawnDuration;
    }
    void StartAgain()
    {
        startSpeed = speed;
        transform.position = pivot.position;
        stepPosition = pivot.position;
        spawnDur = spawnDuration;
    }

    private void FixedUpdate()
    {
        spawnDur -= Time.deltaTime;
        startSpeed = startSpeed * drug;
        transform.position += transform.forward * (startSpeed * Time.deltaTime);

        Vector3 heading = transform.position - stepPosition;
        float distance = heading.magnitude;

        if(distance >= spawnRate && spawnDur > 0)
        {
            if (craterPrefab != null)
            {
                Vector3 pos = transform.position;
                /*if (Terrain.activeTerrains != null)
                {
                    pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
                }*/
                GameObject craterInstance = Instantiate(craterPrefab, pos, Quaternion.identity);
                ParticleSystem craterPS = craterInstance.GetComponent<ParticleSystem>();
                if (craterPS != null)
                {
                    Destroy(craterInstance, craterPS.main.duration);
                }
                else
                {
                    ParticleSystem flashPSParts = craterInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(craterInstance, craterPS.main.duration);
                }
            }
            //distance = 0;
            stepPosition = transform.position;
        }
    }



}
