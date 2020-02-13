using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInspector : MonoBehaviour
{

    [SerializeField]
    float speed;
    [SerializeField]
    GameObject[] models;

    [SerializeField]
    List<Transform> gameObjectList;

    int index = 0;

    private void Awake()
    {
        
    }
    private void Start()
    {
        gameObjectList = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        List<Transform> removeObjects = new List<Transform>();
        foreach (Transform t in gameObjectList)
        {

            if (t.parent == transform)
                t.gameObject.SetActive(false);
            
            else
                removeObjects.Add(t);
            

        }
        foreach (Transform t in removeObjects)
        {
            gameObjectList.Remove(t);
        }

        gameObjectList[0].gameObject.SetActive(true);
        
    }
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (index != 0)
            {
                gameObjectList[index].gameObject.SetActive(false);
                index--;
                gameObjectList[index].gameObject.SetActive(true);
            }

        }else if (Input.GetKeyDown(KeyCode.D))
        {
            if(index < gameObjectList.Count-1)
            {
                gameObjectList[index].gameObject.SetActive(false);
                index++;
                gameObjectList[index].gameObject.SetActive(true);
            }
        }
    }
}
