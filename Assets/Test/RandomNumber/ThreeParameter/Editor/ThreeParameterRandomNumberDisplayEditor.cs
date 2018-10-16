using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ThreeParameterRandomNumberDisplay))]
public class ThreeParameterRandomNumberDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawDisplayButton();
    }

    void DrawDisplayButton()
    {
        if (GUILayout.Button("Display"))
        {
            ThreeParameterRandomNumberDisplay display = (ThreeParameterRandomNumberDisplay)target;
            display.DisplayRandomMap();
        }
    }
}
