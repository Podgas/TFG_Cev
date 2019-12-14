using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakeController : MonoBehaviour
{
    [SerializeField]
    PlayerStats playerStats;
    [SerializeField]
    FloatEvent onSakePick;
    [SerializeField]
    float heal;
    [SerializeField]
    AudioLibrary vfx;

    public void OnPickUp()
    {
        if (playerStats.hp.value < playerStats.hp.maxValue)
        {

            onSakePick.Raise(heal);
            vfx.PlayVFX(AudioLibrary.VfxSounds.Drink);

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
