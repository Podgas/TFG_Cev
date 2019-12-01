using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    Renderer mat;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerHitBox")
        {
            Debug.Log("Hit");
            mat.material.SetColor("_Color",Random.ColorHSV());
        }
    }
}
