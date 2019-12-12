using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformNode : MonoBehaviour
{
    [SerializeField]
    BoolEvent onPlayerEnter;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HOLA");
        if (this.tag == "Transform")
            onPlayerEnter.Raise(true);
        else if (this.tag == "Untransform")
            onPlayerEnter.Raise(false);
    }

}
