using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    Vector3 moveDirection;
    float speed;
    Transform myTransform;

    void Start()
    {
        myTransform = transform;
    }


    void Update()
    {


        Move();

        myTransform.Translate(moveDirection * speed * Time.deltaTime);

    }

    void Move()
    {

        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

    }


}
