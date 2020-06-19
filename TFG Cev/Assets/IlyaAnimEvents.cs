using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlyaAnimEvents : MonoBehaviour
{
    [SerializeField]
    PlayerController pc;



    public void CleanAtk1()
    {
        pc.attak1 = false;
    }


    public void CleanAtk2()
    {
        pc.attak1 = false;

    }
    void ActivateVolume()
    {
        pc.damageVolume.SetActive(true);
    }
    void DesactivateVolume()
    {
        pc.damageVolume.SetActive(false);
    }
}
