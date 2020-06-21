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
        Debug.Log("Detected y pase  " + target);
    }

    public void Reset()
    {
        Debug.Log("UH PAPI ");
        currentPosition = null;
        lastSeenPosition = Vector3.zero;
        isSeen = false;
    }

}
