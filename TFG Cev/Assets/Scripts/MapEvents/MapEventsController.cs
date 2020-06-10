using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventsController : MonoBehaviour
{
   
    [SerializeField]
    public List<MapEvent> mapEventList;
    Transform eventPool;
    private void Start()
    {
    }
    public void OnEventEnter(GameObject mapEvent)
    {
        MapEvent me = mapEvent.gameObject.GetComponent<MapEvent>();
        me.SetActive(true);
    }
    public void OnEventExit(GameObject mapEvent)
    {
        MapEvent me = mapEvent.gameObject.GetComponent<MapEvent>();
        me._isActive = false;
    }
    public int SearchEvent(MapEvent eventToSearch)
    {
        int index = -1;
        for (int i=0; i<mapEventList.Count; i++)
        {
            if(mapEventList[i].name == eventToSearch.name)
            {
                index = i;
                break;
            }
        }
        return index;
    }
    

}
