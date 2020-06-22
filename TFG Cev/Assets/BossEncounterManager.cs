using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncounterManager : MonoBehaviour
{

    PlayerStats ps;

    [SerializeField]
    Transform player;
    [SerializeField]
    Animator door;
    [SerializeField]
    GameObject boss;


    public void OnQuestEnter(GameObject bossEvent)
    {

        if(bossEvent.name == "BossEvent")
        {
            Debug.Log("SPAWN");
            OrochiBehave.DestroyAllOrochi();
            boss.GetComponent<Animator>().SetTrigger("spawn");
            //ps.canMove = false;
            StartConversation();
            door.SetBool("isOpen", false);
        }
    }

    public void StartConversation()
    {


        
    }



}
