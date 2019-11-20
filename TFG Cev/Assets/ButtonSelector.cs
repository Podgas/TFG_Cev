using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{

    public Button selectedButton;
    private void OnEnable()
    {
        selectedButton.Select();
    }
}
