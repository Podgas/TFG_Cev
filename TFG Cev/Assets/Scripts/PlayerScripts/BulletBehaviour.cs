﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField]
    float speed;

    Transform spawnTransform;

    Vector3 moveDirection;

    Ray ray;

    private void Awake()
    {

        Vector3 centre = new Vector3(0.5f, 0.5f, 0f);
        ray = Camera.main.ViewportPointToRay(centre);

        spawnTransform = GameObject.Find("FirePoint").transform;


    }

    void Update()
    {
        Vector3 centre = new Vector3(0.5f, 0.5f, 0f);
        ray = Camera.main.ViewportPointToRay(centre);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, 3000f);

        moveDirection = spawnTransform.position - raycastHit.point;
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
