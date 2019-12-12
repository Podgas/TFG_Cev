using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBehaviour : MonoBehaviour
{
    float currentTime;
    [SerializeField]
    float attackTime;
    [SerializeField]
    float destroyTime;


    private void Awake()
    {
        currentTime = 0;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= attackTime)
        {
            gameObject.layer = 0;
        }
        if (currentTime >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

}
