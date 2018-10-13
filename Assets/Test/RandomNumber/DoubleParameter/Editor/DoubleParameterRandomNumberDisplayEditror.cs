using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoubleParameterRandomNumberDisplay))]
public class DoubleParameterRandomNumberDisplayEditror : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawDisplayButton();
    }

    void DrawDisplayButton()
    {
        DoubleParameterRandomNumberDisplay display = (DoubleParameterRandomNumberDisplay)target;

        if (GUILayout.Button("Display"))
            display.DisplayRandomMap();
    }
}
