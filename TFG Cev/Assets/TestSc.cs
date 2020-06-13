using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSc : MonoBehaviour
{
    [SerializeField]
    GameObject sphere;
    Vector3 direction = new Vector3(0, -1, 0);
    [SerializeField]
    LayerMask terrain;
    [SerializeField]
    GameObject damageVolume;
    private void Awake()
    {
        RaycastHit rHit;

        if (Physics.Raycast(transform.position, direction, out rHit, Mathf.Infinity, terrain))
        {
            Debug.DrawLine(transform.position, rHit.point);
            Instantiate(sphere, rHit.point, Quaternion.identity);

            Vector3 normal = rHit.normal;

            transform.localRotation = Quaternion.FromToRotation(transform.up, rHit.normal) * transform.rotation;

            Vector3 multiplier = new Vector3(0, 0.01f, 0);
            transform.position = rHit.point + multiplier;
        }
        StartCoroutine(SpawnDamage());
    }
    private IEnumerator SpawnDamage()
    {
        
        yield return new WaitForSeconds(1.2f);
        damageVolume.SetActive(true);
       
    }
}
