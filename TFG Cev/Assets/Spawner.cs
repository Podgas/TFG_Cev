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

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= spawnRate)
        {
            currentTime = 0;

            Instantiate(prefab, transform.position, Quaternion.identity);

        }

    }


}
