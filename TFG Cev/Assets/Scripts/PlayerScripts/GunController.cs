using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float fireRate = 1;

    [SerializeField]
    [Range(1, 10)]
    private float damage = 1;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    Camera main;

    [SerializeField]
    GameObject bullet;

    private float timer;

    private bool isShooting = false;



    void Update()
    {
        Vector3 centre = new Vector3(0.5f, 0.5f, 0f);
        Ray ray = Camera.main.ViewportPointToRay(centre);

        
        
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            if (Input.GetAxisRaw("Shoot") != 0)
            {

                if (isShooting == false){ 

                    FireGun();
                    isShooting = true;
                }
            }
            if (Input.GetAxisRaw("Shoot") == 0 && isShooting)
            {
                timer = 0;
                isShooting = false;
            }
        }
        
    }

    private void FireGun()
    {
        Vector3 centre = new Vector3(0.5f, 0.5f, 0f);
        Ray ray = Camera.main.ViewportPointToRay(centre);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, 3000f);

        Vector3 direction = ray.direction;

        GameObject.Instantiate(bullet, firePoint.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Vector3 centre = new Vector3(0.5f, 0.5f, 0f);
        Ray ray = Camera.main.ViewportPointToRay(centre);
        RaycastHit raycastHit;
        Physics.Raycast(ray,out raycastHit, 300f);

        Debug.DrawLine(firePoint.position, raycastHit.point
            , Color.red);


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(raycastHit.point, 0.1f);
    }
}
