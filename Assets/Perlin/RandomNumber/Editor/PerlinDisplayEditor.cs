using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomNumberExamplePerlinDisplay))]
public class PerlinDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
            ((RandomNumberExamplePerlinDisplay)target).Genarete();
    }
}
