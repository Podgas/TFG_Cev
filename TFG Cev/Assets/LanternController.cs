using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternController : MonoBehaviour
{
    public GameObject light;
    public SkinnedMeshRenderer skn;
    private void Update()
    {
        if (skn.isVisible)
        {
            light.SetActive(true);
        }
        else
        {
            light.SetActive(false);
        }
    }
}
