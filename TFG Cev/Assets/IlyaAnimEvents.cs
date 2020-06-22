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

    public void Step()
    {
        AudioManager.Instance.PlaySound("pisada",Random.Range(1.1f,0.9f), 0.1f);
    }
    public void Attack()
    {
        AudioManager.Instance.PlaySound("ilyaAttack", Random.Range(0.7f, 1f), 0.05f);
    }
    public void Dodge()
    {
        AudioManager.Instance.PlaySound("dodge", Random.Range(0.7f, 1f), 0.05f);
    }
    public void Climb()
    {
        AudioManager.Instance.PlaySound("climb", Random.Range(0.7f, 1f), 0.05f);
    }
}
