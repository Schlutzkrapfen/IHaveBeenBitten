using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;


[RequireComponent(typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.right, 360, fov.viewRange);
        Vector3 viewAngleA = fov.DirFromAngle((-fov.viewAngle * 0.5f), false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle * 0.5f, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRange);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRange);
    }

}
