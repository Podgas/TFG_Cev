using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MapStatistics", menuName = "Map/Statistics")]
public class MapController : ScriptableObject
{

    public List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> extras = new List<GameObject>();

    public List<GameObject> completedExtras = new List<GameObject>();

    public void ClearLists()
    {
        enemies.Clear();
        extras.Clear();
        completedExtras.Clear();
    }


}
