using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractObject : MonoBehaviour, IInteractObjects
{

    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject mechanism;

    bool isOn = false;

    public virtual void OnInteract()
    {
        Debug.Log("Interaction!");

        anim.SetBool("isOn", true);
        StartCoroutine("StartMechanimAnim");
    }

    public virtual void ActivateMechanism()
    {
        
        mechanism.GetComponent<Animator>().SetBool("isOpen", true);
    }

    IEnumerator StartMechanimAnim()
    {
        yield return new WaitForSeconds(0.5f);
        ActivateMechanism();

    }


}
