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
    public bool isExtra;

    public GameObjectEvent onEventEnter;
    public GameObjectEvent onEventExit;
    public GameObjectEvent tutorialEnter;
    public GameObjectEvent tutorialExit;
    public GameObjectEvent extaEvent;

    public string questLine;


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
        if(other.tag == "Player")
        {
            onEventEnter.Raise(gameObject);
            if (isTutorial)
            {
                tutorialEnter.Raise(gameObject);
            }
            if (isExtra)
            {
                extaEvent.Raise(gameObject);
                Debug.Log("TOCADO");
            }
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onEventExit.Raise(gameObject);
            if (isTutorial)
            {
                tutorialExit.Raise(gameObject);
            }
        }
    }

}
