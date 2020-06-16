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

                currentEQPosition.x += eqPadding.y;
                storepos = _direction* currentEQPosition.x + transform.position;
                storeX = storepos.x;
                if (Physics.Raycast(storepos, Vector3.down, out rhit, 300f, layerGround))
                {
                    go = Instantiate(earthQuake, storepos, earthQuake.gameObject.transform.rotation);
          
                    go.GetComponent<EarthQuakeBehaviour>().parent = false;
                }

                while (currentEQPosition.z < maxEQRange.x)
                {
                    

                    currentEQPosition.z += eqPadding.x;
                    storepos = storepos + Vector3.Cross(_direction, Vector3.up) * currentEQPosition.z;
                    storepos.y = 10;
                    if (Physics.Raycast(storepos, Vector3.down,out rhit,300f, layerGround))
                    {
                        go = Instantiate(earthQuake, rhit.point, earthQuake.gameObject.transform.rotation);
     
                        go.GetComponent<EarthQuakeBehaviour>().parent = false;
                    }

                    storepos = storepos - Vector3.Cross(_direction, Vector3.up) * currentEQPosition.z;
                    storepos.x = storeX;
                    if (Physics.Raycast(storepos, Vector3.down, out rhit, 300f, layerGround))
                    {
                        go = Instantiate(earthQuake, rhit.point, earthQuake.gameObject.transform.rotation);

                        go.GetComponent<EarthQuakeBehaviour>().parent = false;
                    }
                        

                }
                currentEQPosition.z = 0;
                currentEQPosition.x += eqPadding.y;
            }
        }
        
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _direction.y=0;
    }
}
