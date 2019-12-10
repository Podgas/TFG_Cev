using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    [SerializeField]
    private float timeToMove;
    [SerializeField]
    private float speed;

    bool isMoving = true;
    int nextNode;

    private float actualTime;
    [SerializeField]
    private List<Transform> nodes = new List<Transform>();

    static bool canMove = true;

    float delay = 1.5f  ;
    float delayCurrent;

    static List<PlatformController> allPlatforms = new List<PlatformController>();



    private void Awake()
    {
        allPlatforms.Add(this);
    }
    private void Start()
    {
        nodes.Remove(transform);
        nextNode = 1;
    }

    
    private void Update()
    {
        delayCurrent += Time.deltaTime;
        if (!isMoving && delayCurrent >= delay)
        {
            for (int i = 0; i <= allPlatforms.Count-1; i++)
            {
                if (allPlatforms[i].isMoving)
                {
                    canMove = false;
                    break;
                }
                else
                {
                    canMove = true;
                    delayCurrent = 0;
                }
            }
        }
        
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, nodes[nextNode].position) >= 0.1)
                transform.position = Vector3.MoveTowards(transform.position, nodes[nextNode].position, Time.deltaTime * speed);
            else
                isMoving = false;
        }
        else if(canMove)
        {
            actualTime += Time.deltaTime;

            if (actualTime >= timeToMove)
            {
                actualTime = 0;
                isMoving = true;

                if (nextNode == 0)
                    nextNode = 1;
                else nextNode = 0;
            }
        }
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if(other.transform.parent == transform)
                other.transform.parent = null;
        }
    }
}

