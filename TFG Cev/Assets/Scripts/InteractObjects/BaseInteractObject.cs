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

    protected virtual void Start()
    {
        anim.SetBool("isOn", isOn);
    }
    public virtual void OnInteract()
    {
        isOn = !isOn;
        anim.SetBool("isOn", isOn);
        StartCoroutine("StartMechanimAnim");
    }

    public virtual void ActivateMechanism()
    {
        mechanism.GetComponent<Animator>().SetBool("isOn", isOn);
    }

    IEnumerator StartMechanimAnim()
    {
        yield return new WaitForSeconds(0.5f);
        ActivateMechanism();

    }


}
