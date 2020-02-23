using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ONEnableButton : MonoBehaviour
{
    [SerializeField]
    EventSystem es;
    private void OnEnable()
    {
        es.SetSelectedGameObject(this.gameObject);
        this.gameObject.GetComponent<Button>().OnSelect(null);
        this.gameObject.GetComponent<Button>().Select();
    }
}
