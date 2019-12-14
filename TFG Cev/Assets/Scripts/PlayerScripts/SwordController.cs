using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{

    [SerializeField]
    GameObject hitBox;

    bool isEnabled;



    public void EnableHitbox()
    {
        if (!isEnabled)
        {
            isEnabled = true;
            hitBox.SetActive(isEnabled);
        }
            
    }

    public void DisableHitbox()
    {
        if (isEnabled)
        {
            isEnabled = false;
            hitBox.SetActive(isEnabled);
        }
    }

}
