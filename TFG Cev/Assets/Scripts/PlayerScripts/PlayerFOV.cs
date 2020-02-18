using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : FieldOfViewSystem
{
    [Header("Player")]
    [SerializeField]
    float normalDetection;
    [SerializeField]
    float walkDetection;
    [SerializeField]
    PlayerStats playerStats;
    float detectionRadius;
    [SerializeField]
    float speedNoise;
    [SerializeField]
    SphereCollider detectionCollider;

    protected override void Start()
    {
        base.Start();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (playerStats.playerStatus.isMoving)
        {
            if (detectionRadius != walkDetection)
            {
                detectionRadius += speedNoise * Time.deltaTime;
                detectionRadius = Mathf.Min(detectionRadius, walkDetection);
            }
            
        }
        else
        {
            if (detectionRadius != normalDetection)
            {
                detectionRadius -= speedNoise * Time.deltaTime;
                detectionRadius = Mathf.Max(detectionRadius, normalDetection);
            }
        }
        detectionCollider.radius = detectionRadius;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


}
