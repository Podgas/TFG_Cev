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
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 2f);
        moveDirection = ray.direction;

    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
