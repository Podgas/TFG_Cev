using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchInteract : BaseInteractObject
{
    [SerializeField]
    VoidEvent OnPlayerCrouch;

   
    public override void OnInteract()
    {
        OnPlayerCrouch.Raise();
    }
}
