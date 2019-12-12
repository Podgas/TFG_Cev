using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField]
    VoidEvent onBossChamberEnter;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            onBossChamberEnter.Raise();
            gameObject.GetComponent<Animator>().SetBool("isOn",false);
            gameObject.layer = 13;

        }
    }

}
