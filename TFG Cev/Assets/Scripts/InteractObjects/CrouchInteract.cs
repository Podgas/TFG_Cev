using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchInteract : BaseInteractObject
{
    [SerializeField]
    VoidEvent OnPlayerCrouch;

    protected override void Start()
    {

    }

   
    public override void OnInteract()
    {
        OnPlayerCrouch.Raise();
    }
}
