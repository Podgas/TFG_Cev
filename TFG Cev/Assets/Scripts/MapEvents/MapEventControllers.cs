using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventControllers : MonoBehaviour
{

    [SerializeField]
    VoidEvent eventToRaise;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {
            Debug.Log("ENTRING");
            eventToRaise.Raise();
        }
    }

}
