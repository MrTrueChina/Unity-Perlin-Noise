using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PerlinDisplay))]
public class PerlinDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            PerlinDisplay display = (PerlinDisplay)target;
            display.Genarete();
        }
    }
}
