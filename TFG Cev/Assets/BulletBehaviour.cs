using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField]
    float speed;

    Transform spawnTransform;

    Vector3 moveDirection;

    private void Awake()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Vector3 direction = ray.direction;
        direction.x += 0.05f;
        moveDirection = direction;

    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
    }
}
