using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : MonoBehaviour
{

    public string name;
    public Collider volume;
    public bool _isActive = false;
    public bool wasActive = false;
    public bool isTutorial;

    public GameObjectEvent onEventEnter;
    public GameObjectEvent onEventExit;
    public GameObjectEvent tutorialEnter;
    public GameObjectEvent tutorialExit;


    private void Start()
    {
        name = gameObject.name;
    }
   
    public void SetActive(bool isActive)
    {
        
        if (!wasActive)
        {
            _isActive = isActive;
            wasActive = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HOLA");
        onEventEnter.Raise(gameObject);
        if (isTutorial)
        {
            tutorialEnter.Raise(gameObject);
        }


    }
    private void OnTriggerExit(Collider other)
    {
        onEventExit.Raise(gameObject);
        if (isTutorial)
        {
            tutorialExit.Raise(gameObject);
        }
    }
}
