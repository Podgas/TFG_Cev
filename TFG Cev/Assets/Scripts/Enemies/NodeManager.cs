using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{

    private Transform nextNode;


    List<Transform> nodes = new List<Transform>();

    private void Awake()
    {
        GetPatrolNodes();

    }

    void GetPatrolNodes()
    {
        foreach (Transform node in gameObject.GetComponentsInChildren<Transform>())
        {

            nodes.Add(node);

        }
        nodes.RemoveAt(0);
    }

    public Transform GetNode(int index)
    {
        return nodes[index];
    }

    public Transform NextNode(Transform currentNode)
    {
        int currentNodeIndex = nodes.IndexOf(currentNode);
        if (currentNodeIndex == nodes.Count - 1)
        {
            currentNodeIndex = 0;
            return nodes[currentNodeIndex];
        }
        else
        {
            currentNodeIndex++;
            return nodes[currentNodeIndex];
        }
    }
}
