using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{

    [SerializeField]
    private GameObject destroyedObject;

    bool isBreaked = false;

    private void OnTriggerEnter(Collider other)
    {


        if(other.tag == "PlayerHitBox" && !isBreaked)
        {
            Instantiate(destroyedObject, transform.position, transform.rotation);
            isBreaked = true;
            Destroy(gameObject);
        }
    }
}
