using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{

    [SerializeField]
    private GameObject destroyedObject;

    [SerializeField]
    GameObject[] drops;


    bool isBreaked = false;
    [SerializeField]
    MeshRenderer meshRenderer;


    private void OnTriggerEnter(Collider other)
    {


        if(other.tag == "PlayerHitBox" && !isBreaked)
        {
            isBreaked = true;
            GameObject breaked = Instantiate(destroyedObject, transform.position, transform.rotation);
            meshRenderer.enabled = false;
            breaked.transform.parent = gameObject.transform;

            float random = Random.Range(0, 999);
            if(random <= 100)
            {
                Instantiate(drops[0], transform.position, Quaternion.identity);
            }
            else if(random <= 200 && random > 100)
            {
                Instantiate(drops[1], transform.position, Quaternion.identity);
            }

            StartCoroutine("DesrtoyBreakedObject");
        }
    }

    IEnumerator DestroyBreakedObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
