using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInspector : MonoBehaviour
{

    [SerializeField]
    float speed;
    [SerializeField]
    GameObject[] models;

    List<GameObject> gameObjectList;


    private void Awake()
    {
        if (models.Length >= 0)
        {
            GameObject o = null;
            foreach(GameObject model in models)
            {
                o = Instantiate(model, transform.position, Quaternion.identity);
                gameObjectList.Add(o);
                o.transform.parent = gameObject.transform;
            }

        }
    }

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
