using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractObject : MonoBehaviour, IInteractObjects
{
    [SerializeField]
    private float interactRadius;
    [SerializeField]
    Animator anim;

    bool isOn = false;

    private void Start()
    {
        anim.SetBool("isOn", isOn);
    }
    public void OnInteract()
    {
        isOn = !isOn;
        anim.SetBool("isOn", isOn);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

}
