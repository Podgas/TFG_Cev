using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class EarthQuakeBehaviour : MonoBehaviour
{
    [SerializeField]
    ParticleSystem earthQuake;
    [SerializeField]
    Vector2 eqPadding;
    [SerializeField]
    Vector2 maxEQRange;
    [SerializeField]
    float eartquakeDelay;
    [SerializeField]
    LayerMask layerGround;
    Vector3 currentEQPosition;
    float storeX;
    Vector3 storepos;
    ParticleSystem go;
    public bool parent;
    [SerializeField]
    Vector3 _direction;

    float currentTime;
    void Start()
    {
        Destroy(this, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (parent)
        {
            currentTime += Time.deltaTime;
            if (currentTime > eartquakeDelay)
            {
                Vector3 finalPoint;
                RaycastHit rhit;
                currentTime = 0;

                currentEQPosition.z += eqPadding.y;
                storepos = currentEQPosition + transform.position + Vector3.up * 5;
                storeX = storepos.x;
                if (Physics.Raycast(storepos, Vector3.down, out rhit, 300f, layerGround))
                {

                    go = Instantiate(earthQuake, rhit.point, earthQuake.gameObject.transform.rotation);
          
                    go.GetComponent<EarthQuakeBehaviour>().parent = false;
                    Destroy(go.gameObject, 1.5f);
                }

                while (currentEQPosition.x < maxEQRange.x)
                {
                    currentEQPosition.x += eqPadding.x;
                    storepos = transform.position + currentEQPosition + Vector3.up * 5;
                    if (Physics.Raycast(storepos, Vector3.down,out rhit,300f, layerGround))
                    {
                        go = Instantiate(earthQuake, rhit.point, earthQuake.gameObject.transform.rotation);
     
                        go.GetComponent<EarthQuakeBehaviour>().parent = false;
                        Destroy(go.gameObject, 1.5f);
                    }

                    currentEQPosition.x = -currentEQPosition.x;
                    storepos = transform.position + currentEQPosition + Vector3.up * 5;
                    if (Physics.Raycast(storepos, Vector3.down, out rhit, 300f, layerGround))
                    {
                        go = Instantiate(earthQuake, rhit.point, earthQuake.gameObject.transform.rotation);

                        go.GetComponent<EarthQuakeBehaviour>().parent = false;
                        Destroy(go.gameObject, 1.5f);
                    }
                    currentEQPosition.x = -currentEQPosition.x;



                }
                currentEQPosition.x = 0;
            }
        }
        
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _direction.y=0;
    }
}
