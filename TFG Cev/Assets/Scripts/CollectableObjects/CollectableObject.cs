using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour, IInteractObjects
{

    [SerializeField]
    GameObjectEvent onCollect; 

    public void OnInteract()
    {
        onCollect.Raise(gameObject);
        gameObject.SetActive(false);
    }
}
