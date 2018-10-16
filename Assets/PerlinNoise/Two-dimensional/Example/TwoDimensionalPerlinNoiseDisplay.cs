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
    bool _evaluate = false;
    


    public void GeneratePerlinNoise()
    {
        float[,] perlinMap = GetPerlinMap(_length, _length, _offset);

        if (_evaluate)
            perlinMap = BeautifyPerlinMap(perlinMap);

        Texture2D texture = GetTextureFromRandomMap(perlinMap);
        _renderer.sharedMaterial.mainTexture = texture;
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
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, randomMap[x, y]);
            }

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }



    static float[,] BeautifyPerlinMap(float[,] map)
    {
        float min = float.MaxValue;
        float max = float.MinValue;

        foreach (float perlinValue in map)
        {
            if (perlinValue < min)
                min = perlinValue;
            if (perlinValue > max)
                max = perlinValue;
        }

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                map[x, y] = Mathf.InverseLerp(min, max, map[x, y]);

        return map;
    }


    private void OnValidate()
    {
        if (_length > 1000) _length = 1000;
        if (_length < 1) _length = 1;
        if (_scale < 0.01f) _scale = 0.01f;

        GeneratePerlinNoise();
    }
}
