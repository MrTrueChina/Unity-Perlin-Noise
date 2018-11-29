using UnityEditor;
using UnityEngine;

namespace MtC.Tools.PerlinNoise
{
    [CustomEditor(typeof(RandomPerlinNoiseDisplay))]
    public class RandomPerlinNoiseDisplayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
                ((RandomPerlinNoiseDisplay)target).Genarete();
        }
    }
}