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

    public void OnPickUp()
    {
        if (playerStats.hp.value < playerStats.hp.maxValue)
        {

            onSakePick.Raise(heal);
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
