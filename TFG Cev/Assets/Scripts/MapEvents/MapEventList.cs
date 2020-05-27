using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapEventList : MonoBehaviour
{
    [System.Serializable]
    public class MapEvent
    {
        public GameObject volume;
        public bool isActive;
        public string name;
        public bool wasActive;


        public void SetActive(bool b)
        {
            isActive = b;
        }

        public void SetWasActive()
        {
            wasActive = true;
        }
    }

    [SerializeField]
    public List<MapEvent> eventList;
    [SerializeField]
    HUDController hud;

    public void OnEventStart(GameObject eventToSearch)
    {
        int index;
        index = SearchForEvent(eventToSearch);
        if (!eventList[index].wasActive)
        {
            eventList[index].SetActive(true);
            hud.OnTutorialActivation(eventList[index].name);
        }
            
    }

    public void OnEventEnd(GameObject eventToSearch)
    {
        int index;
        index = SearchForEvent(eventToSearch);
        if (!eventList[index].wasActive)
        {
            eventList[index].SetWasActive();
            eventList[index].SetActive(false);
            hud.OnTutorialDeactivation();
        }
            
    }

    public int SearchForEvent(GameObject eventToSearch)
    {
        for (int i=0; eventList.Count > i; i++)
        {
            if (eventList[i].volume == eventToSearch)
            {
                return i;
            }
        }
        return -1;
    }

}
