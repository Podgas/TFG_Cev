using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventClass : MonoBehaviour
{

    [SerializeField]
    GameObjectEvent onEventEnter;
    [SerializeField]
    GameObjectEvent onEventExit;
    [SerializeField]
    bool isTutorial;


    private void OnTriggerEnter(Collider other)
    {
        onEventEnter.Raise(gameObject);
    }


    private void OnTriggerExit(Collider other)
    {
        onEventExit.Raise(gameObject);
    }
}
