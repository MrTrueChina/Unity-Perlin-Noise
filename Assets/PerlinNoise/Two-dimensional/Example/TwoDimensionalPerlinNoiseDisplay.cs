using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalPerlinNoiseDisplay : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _renderer;

    [SerializeField]
    int _seed;
    [SerializeField]
    int _length = 100;
    [SerializeField]
    Vector2 _offset;
    [SerializeField]
    float _scale = 1;
    [SerializeField]
    [Range(1, 5)]
    int _octaves = 3;
    [SerializeField]
    [Range(0, 1)]
    float _persistence = 0.5f;
    [SerializeField]
    float _lacunarity = 3;      //这个数大小正负应该都行，但小于1会导致后续噪声越来越平坦，因为随机点距离越来越远了

    [SerializeField]
    bool _drawPerlinNoise = true;

    [SerializeField]
    bool _drawSubPerlinNoise = false;       //绘制子柏林噪声，也就是计算总柏林噪声时计算的小噪声
    [SerializeField]
    int _drawSubPerlinNoiseIndex;           //绘制的子噪声的下标
    


    public void GeneratePerlinNoise()
    {
        float[,] perlinMap = GetPerlinMap(_length, _length, _offset);
        Texture2D texture = GetTextureFromRandomMap(perlinMap);
        _renderer.material.mainTexture = texture;
    }

    float[,] GetPerlinMap(int width, int height, Vector2 offset)
    {
        float[,] map = new float[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = TwoDimensionalPerlinNoiseGenerator.GetPerlinValue(_seed, x + offset.x, y + _offset.y, _scale, _octaves, _persistence, _lacunarity);

        return map;
    }

    Texture2D GetTextureFromRandomMap(float[,] randomMap)
    {
        int width = randomMap.GetLength(0);
        int height = randomMap.GetLength(1);

        Color[] colorMap = new Color[width * height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, randomMap[x, y]);

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    /*
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
    */


    private void OnValidate()
    {
        if (_length > 1000) _length = 1000;
        if (_length < 1) _length = 1;
        if (_scale < 0.01f) _scale = 0.01f;

        GeneratePerlinNoise();
    }
}
