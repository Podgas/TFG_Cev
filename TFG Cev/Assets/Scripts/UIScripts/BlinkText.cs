using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    Text text;
    [SerializeField]
    float startTime;
    float actualTime;
    public float blinkTime = 0.5f;

    bool blinkIn = true;

    private void Start()
    {
        text = GetComponent<Text>();
        text.canvasRenderer.SetAlpha(0);
    }

    private void Update()
    {


        if (startTime >= 0)
        {
            Blink();
        }
        else
        {
            startTime += Time.deltaTime;
        }
        
        
    }

    void Blink()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= blinkTime)
        {
            blinkIn = !blinkIn;
            actualTime = 0;
        }
        if (blinkIn)
        {
            text.CrossFadeAlpha(1, blinkTime, false);
        }
        else
            text.CrossFadeAlpha(0, blinkTime, false);
    }

}
