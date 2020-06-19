using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpostorBehave : MonoBehaviour
{

    [SerializeField]
    Renderer geo;

    bool isDissolving = true;
    float currentTime = 0f;
    float timeToMove = 2f;

    void Update()
    {


            

    }
    private void Start()
    {
    }
    /*
    private void Update()
    {
        currentDissolveTime += Time.deltaTime;


        geo.material.SetFloat("_dissolve", Mathf.Lerp(dissolve,1,Time.deltaTime * dissolveTime));
    }*/


}
