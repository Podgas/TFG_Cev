using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Map Event", order = 52)]

public class MapEventList : ScriptableObject
{
    public Dictionary<string,bool> eventCheck;
}
