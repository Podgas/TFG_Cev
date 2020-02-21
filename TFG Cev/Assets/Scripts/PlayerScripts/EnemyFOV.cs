using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class EnemyFOV : FieldOfViewSystem
{
    
    
    MaterialPropertyBlock block;


    public Color freeColor;
    public Color alertColor;
    public Color findColor;
    public Color detectedColor;

    [Header("Enemy")]
    [SerializeField]
    float aggresionRadius;
    [SerializeField]
    float detectionValue;
    [SerializeField]
    GameObject go;
    public Color color;
    [SerializeField]
    EnemyBase enemyBehave;

    float distance;

    public float timeToDetect;
    public float actualTime;

    //booleanStates
    public bool isPatroling = true;
    [Task]
    public bool isAlert = false;
    public bool isChasing = false;

    float detectionLerpSolver;
    public float soundDetectionRadius;
    

    protected override void Start()
    {
        base.Start();
        block = new MaterialPropertyBlock();
        color = freeColor;
        block.SetColor("_BaseColor", color);
        go.GetComponent<Renderer>().SetPropertyBlock(block);
       
    }

    protected override void LateUpdate()
    {
       
        base.LateUpdate();
        ListenTargets();

        if (isChasing)
        {
            color = detectedColor;
            Collider[] targetForChase = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            if (targetForChase.Length == 0)
            {
                isChasing = false;
                isPatroling = true;
                color = freeColor;
            }
        }
        else
        {
            if (alertedTargets.Count > 0 && visibleTargets.Count == 0)
            {
                
                color = alertColor;
                if (isPatroling)
                {
                    Debug.Log("ALERT");
                    StateTransition(ref isPatroling, ref isAlert);
                    OnObjectAlert(alertedTargets[0].gameObject);
                }
            }
            else if (visibleTargets.Count > 0)
            {
                distance = Vector3.Distance(transform.position + new Vector3(aggresionRadius, 0, 0),
                    visibleTargets[0].position);

                actualTime += Time.deltaTime / (distance / (viewRadius - aggresionRadius));
                color = findColor;
                if (actualTime >= timeToDetect)
                {
                    actualTime = 0;
                    StateTransition(ref isAlert, ref isChasing);
                    OnObjectDetected(visibleTargets[0].gameObject);
                }
            }
            else if (!isPatroling && !isChasing)
            {
                ResetStates(ref isAlert, ref isChasing, ref isPatroling);
                color = freeColor;
                actualTime = 0;
                OnObjectOut();
            }
        }

        block.SetColor("_BaseColor", color);
        go.GetComponent<Renderer>().SetPropertyBlock(block);
    }

    public void OnObjectDetected(GameObject objectDetected)
    {
        enemyBehave.OnDetected(objectDetected);
    }
    public void OnObjectAlert(GameObject objectDetected)
    {
        enemyBehave.SetAlert(objectDetected.transform.position);
    }
    public void OnObjectOut()
    {
        enemyBehave.SetCalm();
    }

    private void StateTransition(ref bool initState,ref bool newState)
    {
        initState = !initState;
        newState = !newState;

    }
    private void ResetStates(ref bool state1, ref bool state2 , ref bool newState)
    {
        state1 = false;
        state2 = false;
        newState = true;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position + new Vector3(aggresionRadius, 0, 0), 2f);
        //Gizmos.DrawWireSphere(transform.position , soundDetectionRadius);
    }

    void ListenTargets()
    {
        Collider[] targetInListenRadius = Physics.OverlapSphere(transform.position, soundDetectionRadius, targetMask);
        
        for (int i = 0; i < targetInListenRadius.Length; i++)
        {
            
            Vector3 target = targetInListenRadius[i].transform.position;

            Vector3 dirToTarget = (target - transform.position).normalized;
            Chase();
            OnObjectDetected(targetInListenRadius[i].gameObject);
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                Chase();
            }
        }
    }

    void Chase()
    {
        isChasing = true;
        isAlert = false;
        isPatroling = false;
    }
}
