using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfViewComponent))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfViewComponent fov = (FieldOfViewComponent)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRaidus);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRaidus);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRaidus);
        Handles.color = Color.blue;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}
