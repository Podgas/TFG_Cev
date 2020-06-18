using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForestEvents", menuName = "MapEvents", order = 52)]



public class MapEventList : ScriptableObject
{
    [System.Serializable]
    public struct MapEvent
    {
        public bool isActive;
        public string name;
        public BoxCollider zone;
    }

    [SerializeField]
    public List<MapEvent> eventList;
}
