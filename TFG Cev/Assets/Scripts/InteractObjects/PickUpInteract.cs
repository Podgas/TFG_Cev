using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteract : MonoBehaviour
{
    [SerializeField]
    float fallSpeed;
    [SerializeField]
    float hitDistance;
    [SerializeField]
    LayerMask groundHitLayer;

    Vector3 _moveDirection;
    bool isGrounded = true;
    


    public Transform OnInteract()
    {

        return this.transform;
    }

    private void Update()
    {

    }



}
