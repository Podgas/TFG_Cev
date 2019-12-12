using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBehaviour : MonoBehaviour
{
    [SerializeField]
    float speed;


    private void Awake()
    {
        StartCoroutine("DestroyAttack");
    }

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);    
    }

    IEnumerator DestroyAttack()
    {
        yield return new WaitForSeconds(4);
    }
}
