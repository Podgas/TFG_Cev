using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    float spawnRate;


    float currentTime;

    [SerializeField]
    bool isOneSpawn;

    private void Start()
    {
        if (isOneSpawn)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
    private void Update()
    {
        
        if(!isOneSpawn)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= spawnRate)
            {
                currentTime = 0;

                Instantiate(prefab, transform.position, Quaternion.identity);

            }
        }
        

    }


}
