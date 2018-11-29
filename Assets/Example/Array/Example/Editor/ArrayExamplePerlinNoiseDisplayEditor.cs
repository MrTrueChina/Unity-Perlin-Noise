using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArrayExamplePerlinNoiseDisplay))]
public class ArrayExamplePerlinNoiseDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
            ((ArrayExamplePerlinNoiseDisplay)target).Genarete();
    }
}
