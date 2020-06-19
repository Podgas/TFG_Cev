using UnityEngine;

[System.Serializable]
public class Target 
{
    public Transform currentPosition;
    public Vector3 lastSeenPosition;
    public bool isSeen = false;


    public void Detect(Transform target)
    {
        currentPosition = target;
        lastSeenPosition = target.position;
        isSeen = true;
    }

    public void Reset()
    {
        currentPosition = null;
        lastSeenPosition = Vector3.zero;
        isSeen = false;
    }

}
