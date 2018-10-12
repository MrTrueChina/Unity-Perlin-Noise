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
    [SerializeField]
    [Range(1, 5)]
    int _octaves = 1;
    [SerializeField]
    [Range(0, 1)]
    float _persistence;
    [SerializeField]
    float _lacunarity;      //这个数大小正负应该都行，但小于1会导致后续噪声越来越平坦，因为随机点距离越来越远了

    [SerializeField]
    bool _drawPerlinNoise = true;

    [SerializeField]
    bool _drawSubPerlinNoise = false;       //绘制子柏林噪声，也就是计算总柏林噪声时计算的小噪声
    [SerializeField]
    int _drawSubPerlinNoiseIndex;           //绘制的子噪声的下标


    private void OnDrawGizmos()
    {
        if (_drawPerlinNoise) DrawPerlinNoise();
        if (_drawSubPerlinNoise) DrawSubPerlinNoise();
    }

    void DrawPerlinNoise()
    {
        Gizmos.color = Color.black;

        Vector3[] positions = new Vector3[_steps];
        for (int i = 0; i < _steps; i++)
        {
            float originXPos = _long / _steps * i;
            float perlinXPos = originXPos + _offset;
            float perlinValue = OneDimensionalPerlinNoiseGenerator.GetPerlinValue(_seed, perlinXPos, _scale, _octaves, _persistence, _lacunarity);
            positions[i] = new Vector3(originXPos, perlinValue, 0) + transform.position;
        }

        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i], positions[i + 1]);

        float endXPos = transform.position.x + _long;
        Vector3 endPosition = new Vector3(_long, OneDimensionalPerlinNoiseGenerator.GetPerlinValue(_seed, endXPos, _scale, _octaves, _persistence, _lacunarity)) + transform.position;
        Gizmos.DrawLine(positions[positions.Length - 1], endPosition);
    }

    void DrawSubPerlinNoise()
    {
        Gizmos.color = Color.blue;

        Vector3[] positions = new Vector3[_steps];
        for (int i = 0; i < _steps; i++)
        {

            float originXPos = _long / _steps * i;
            float perlinXPos = originXPos + _offset;
            float perlinValue = OneDimensionalPerlinNoiseGenerator.GetSubPerlinValue(_seed, perlinXPos, _scale, _persistence, _lacunarity, _drawSubPerlinNoiseIndex);
            positions[i] = new Vector3(originXPos, perlinValue, 0) + transform.position;
        }

        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i], positions[i + 1]);

        float endXPos = transform.position.x + _long;
        Vector3 endPosition = new Vector3(_long, OneDimensionalPerlinNoiseGenerator.GetSubPerlinValue(_seed, endXPos, _scale, _persistence, _lacunarity, _drawSubPerlinNoiseIndex)) + transform.position;
        Gizmos.DrawLine(positions[positions.Length - 1], endPosition);
    }


    private void OnValidate()
    {
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
