using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(TwoDimensionalPerlinNoiseDisplay))]
public class TwoDimensionalPerlinNoiseDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawGenerateButton();
    }

    void DrawGenerateButton()
    {
        if (GUILayout.Button("Generate"))
        {
            TwoDimensionalPerlinNoiseDisplay display = (TwoDimensionalPerlinNoiseDisplay)target;
            display.GeneratePerlinNoise();
        }
    }
}
