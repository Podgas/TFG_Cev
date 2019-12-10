using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [SerializeField]
    PlayerStats playerStats;
    [SerializeField]
    VoidEvent onAmmoChange;

    public void OnPickUp()
    {
        if(playerStats.ammo < playerStats.maxAmmo)
        {
            if (playerStats.maxAmmo - playerStats.ammo == 1)
                playerStats.ammo++;
            else
                playerStats.ammo += 2;

            onAmmoChange.Raise();
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPickUp();
        }
    }

}
