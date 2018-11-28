using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SingleParameterRandomNumberDisplay))]
public class SingleParameterRandomNumberDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
            ((SingleParameterRandomNumberDisplay)target).Generate();
    }
}
