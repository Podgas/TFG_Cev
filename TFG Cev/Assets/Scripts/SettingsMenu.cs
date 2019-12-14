using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{


    [SerializeField]
    Dropdown resolutionDropdown;
    [SerializeField]
    Vector2[] screenRes;

    private void Start()
    {
       
    }

    public void SetQuality(int qualityIndex)
    {

        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SerResolution(int resolutionIndex)
    {

        Screen.SetResolution((int)screenRes[resolutionIndex].x, (int)screenRes[resolutionIndex].y, true);

    }
}
