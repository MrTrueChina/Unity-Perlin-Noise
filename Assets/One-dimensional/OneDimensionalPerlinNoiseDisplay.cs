using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDimensionalPerlinNoiseDisplay : MonoBehaviour
{
    [SerializeField]
    int _seed;
    [SerializeField]
    float _long = 3;
    [SerializeField]
    int _steps = 100;
    [SerializeField]
    float _offset;
    [SerializeField]
    float _scale = 1;


    private void OnDrawGizmos()
    {
        DrawPerlinNoise();
    }

    void DrawPerlinNoise()
    {
        Gizmos.color = Color.black;

        Vector3[] positions = new Vector3[_steps];
        for (int i = 0; i < _steps; i++)
        {

            float originXPos = _long / _steps * i;
            float perlinXPos = originXPos + _offset;
            float perlinValue = OneDimensionalPerlinNoiseGenerator.GetSinglePerlinValue(_seed, perlinXPos, _scale);
            positions[i] = new Vector3(originXPos, perlinValue, 0) + transform.position;
        }

        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i], positions[i + 1]);

        float endXPos = transform.position.x + _long;
        Vector3 endPosition = new Vector3(_long, OneDimensionalPerlinNoiseGenerator.GetSinglePerlinValue(_seed, endXPos, _scale)) + transform.position;
        Gizmos.DrawLine(positions[positions.Length - 1], endPosition);
    }


    private void OnValidate()
    {
        if (_scale < 0.01f) _scale = 0.01f;
        if (_steps > 1000) _steps = 1000;
        if (_long <= 0) _long = 0;
    }



    //下面是测试阶段的代码，留下来好做测试用
    void DrawCos()
    {
        Gizmos.color = Color.red;

        Vector3[] positions = new Vector3[_steps];
        for (int i = 0; i < _steps; i++)
        {
            float x = _long / _steps * i;
            float cosValue = Mathf.Cos(x);
            positions[i] = new Vector3(x, cosValue, 0);
        }

        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i], positions[i + 1]);
    }

    void DrawSin()
    {
        Gizmos.color = Color.blue;

        Vector3[] positions = new Vector3[_steps];
        for (int i = 0; i < _steps; i++)
        {
            float x = _long / _steps * i;
            float sinValue = Mathf.Sin(x);
            positions[i] = new Vector3(x, sinValue, 0);
        }

        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i], positions[i + 1]);
    }
}
