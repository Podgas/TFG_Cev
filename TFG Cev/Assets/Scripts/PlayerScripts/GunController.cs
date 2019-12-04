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
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.Log("hi");
        Vector3 direction = ray.direction;
        direction.x += 0.05f;
        Debug.DrawRay(firePoint.position, direction*100, Color.red,2f);
        GameObject.Instantiate(bullet, firePoint.position, Quaternion.identity);
    }
}
