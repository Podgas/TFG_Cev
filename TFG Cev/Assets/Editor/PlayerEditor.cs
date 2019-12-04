using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (PlayerController))]
public class PlayerEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerController fow = (PlayerController)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position,Vector3.up, fow.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        foreach(Transform visibleTargets in fow.interactableTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTargets.position);
        }
    }
}
