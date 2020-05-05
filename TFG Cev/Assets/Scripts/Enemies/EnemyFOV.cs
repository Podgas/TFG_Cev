using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : FieldOfViewSystem
{
    
    MaterialPropertyBlock block;


    [Header("Enemy")]
    [SerializeField]
    float aggresionDistance;
    [SerializeField]
    float timeToDetect;
    [SerializeField]
    float actualTime;
    [SerializeField]
    EnemyBase baseMotor;

    float correctedDistance;
    float aggresionResidue;

    [Header("Enemy Mesh Colors")]
    [SerializeField]
    GameObject viewCone;
    public Color color;

    public Color freeColor;
    public Color alertColor;
    public Color detectedColor;

    float distance;
    Target _target = new Target();

    //SoundDetection
    float detectionLerpSolver;
    public float soundDetectionRadius;

    bool isAlert = false;
    bool isChasing = false;

    protected override void Start()
    {
        base.Start();

        aggresionResidue = viewRadius - aggresionDistance;

        block = new MaterialPropertyBlock();
        color = freeColor;
        block.SetColor("_BaseColor", color);
        viewCone.GetComponent<Renderer>().SetPropertyBlock(block);
       
    }

    protected override void LateUpdate()
    {
       
        base.LateUpdate();
        //ListenTargets();
        if (!isChasing)
        {
            if(visibleTargets.Count > 0)
            {
                _target.currentPosition = visibleTargets[0];
                if (!isAlert)
                {
                    _target.lastSeenPosition = visibleTargets[0].position;
                    isAlert = true;
                    baseMotor.OnAlert(_target);

                    distance = Vector3.Distance(transform.position,
                        visibleTargets[0].position);

                    correctedDistance = distance -= aggresionResidue;
                    color = alertColor;
                }
                else
                {
                    actualTime += Time.deltaTime / (distance / correctedDistance);
                    if (actualTime >= timeToDetect)
                    {
                        isChasing = true;
                        actualTime = 0;

                        _target.Detect(visibleTargets[0]);
                        baseMotor.OnDetect(_target);

                        color = detectedColor;
                    }
                }
            }
            else
            {
                if (isAlert)
                {
                    _target.Reset();
                    baseMotor.OnCalm(_target);
                    isAlert = false;
                    color = freeColor;
                    actualTime = 0;
                }
            }
        }

        block.SetColor("_BaseColor", color);
        viewCone.GetComponent<Renderer>().SetPropertyBlock(block);

    }

    /*void ClearBools()
    {
        isChasing = false;
        isAlert = false;
    }*/

    void ListenTargets()
    {
        Collider[] targetInListenRadius = Physics.OverlapSphere(transform.position, soundDetectionRadius, targetMask);
        
        for (int i = 0; i < targetInListenRadius.Length; i++)
        {
            
            Vector3 target = targetInListenRadius[i].transform.position;

            Vector3 dirToTarget = (target - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {

            }
        }
    }

    public void OnTutorial()
    {
        isAlert = true;
        isChasing = true;
    }

}
