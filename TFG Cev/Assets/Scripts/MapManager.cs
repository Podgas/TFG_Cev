using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [Header("LevelConditions")]
    [SerializeField]
    GameObject losePanel;


    [Header("MapAttributes")]
    [SerializeField]
    GameObject collectablesContainer;
    [SerializeField]
    MapController mc;
    [SerializeField]
    VoidEvent onExtrasListUpdate;

    float timeToFade=1f;
    float actualTime;

    private void Start()
    {
        /*mc.ClearLists();
        foreach (CollectableObject cObject in collectablesContainer.GetComponentsInChildren<CollectableObject>())
        {

            mc.extras.Add(cObject.gameObject);

        }*/

    }

    

    public void OnCollectablePickUp(GameObject collectable)
    {
        mc.completedExtras.Add(collectable);
        onExtrasListUpdate.Raise();
    }

    public void LoseScreen()
    {
        losePanel.SetActive(true);
        Animator anim = losePanel.GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

}
