using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomNumberPerlinDisplay))]
public class PerlinDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
            ((RandomNumberPerlinDisplay)target).Genarete();
    }
}
