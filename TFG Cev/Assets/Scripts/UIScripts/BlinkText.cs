using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    Color textColor;
    [SerializeField]
    float actualTime;
    float blinkTime = 0.5f;
    bool blinkIn = true;

    private void Start()
    {
        textColor = GetComponent<Text>().material.color;
    }

    private void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= blinkTime)
        {
            blinkIn = !blinkIn;
            actualTime = 0;
        }
        if (blinkIn)
        {
            textColor.a = 0;
        }
        else
            textColor.a = 1;

    }
}
