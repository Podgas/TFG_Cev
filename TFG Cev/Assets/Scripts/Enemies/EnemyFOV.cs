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


    //SoundDetection
    float detectionLerpSolver;
    public float soundDetectionRadius;

    //EVENTS
    [Header("Events")]
    [SerializeField]
    PackageEvent playerIsAlert;
    [SerializeField]
    PackageEvent playerIsDetected;
    [SerializeField]
    PackageEvent calmIsReached;
    [SerializeField]
    public EventPackage packageEvent = new EventPackage();

    bool isAlert = false;
    bool isChasing = false;



    protected override void Start()
    {
        base.Start();

        aggresionResidue = viewRadius - aggresionDistance;
        packageEvent.instance = gameObject;

        block = new MaterialPropertyBlock();
        color = freeColor;
        block.SetColor("_BaseColor", color);
        viewCone.GetComponent<Renderer>().SetPropertyBlock(block);
       
    }

    protected override void LateUpdate()
    {
       
        base.LateUpdate();
        //ListenTargets();

        if (visibleTargets.Count > 0 && !isChasing)
        {
            if (!isAlert)
            {
                packageEvent.target = visibleTargets[0];
                isAlert = true;
                playerIsAlert.Raise(packageEvent);

                distance = Vector3.Distance(transform.position,
                    visibleTargets[0].position);

                correctedDistance = distance -= aggresionResidue;
                color = alertColor;
            }
            else if (!isChasing)
            {
                actualTime += Time.deltaTime / (distance / correctedDistance);
                if (actualTime >= timeToDetect)
                {
                    isChasing = true;
                    actualTime = 0;
                    playerIsDetected.Raise(packageEvent);
                    color = detectedColor;
                }
                actualTime += Time.deltaTime / (distance / correctedDistance);
            }
        }
        else if(!isChasing){
            if (isAlert)
            {
                calmIsReached.Raise(packageEvent);
                isAlert = false;
                color = freeColor;
                actualTime = 0;
                packageEvent.target = null;
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
