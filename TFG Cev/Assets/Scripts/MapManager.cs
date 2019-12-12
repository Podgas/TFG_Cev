using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    GameObject collectablesContainer;
    [SerializeField]
    MapController mc;
    [SerializeField]
    VoidEvent onExtrasListUpdate;

    private void Start()
    {
        mc.ClearLists();
        foreach (CollectableObject cObject in collectablesContainer.GetComponentsInChildren<CollectableObject>())
        {

            mc.extras.Add(cObject.gameObject);

        }

    }

    public void OnCollectablePickUp(GameObject collectable)
    {
        mc.completedExtras.Add(collectable);
        onExtrasListUpdate.Raise();
    }

}
