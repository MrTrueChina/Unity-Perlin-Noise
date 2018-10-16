using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeParameterRandomNumberDisplay : MonoBehaviour
{
    [SerializeField]
    Renderer _XYDisplayRenderer;
    [SerializeField]
    Renderer _YZDisplayRenderer;
    [SerializeField]
    Renderer _XZDisplayRenderer;
    [SerializeField]
    Text _XYAverageText;
    [SerializeField]
    Text _YZAverageText;
    [SerializeField]
    Text _XZAverageText;

    [SerializeField]
    int _length;
    [SerializeField]
    Vector3Int _offset;

    
    public void DisplayRandomMap()
    {
        DisplayXYRandomMap();
        DisplayYZRandomMap();
        DisplayXZRandomMap();
    }

    void DisplayXYRandomMap()
    {
        float[,] randomMap = GetXYRandomMap(_length, _length, _offset);
        Texture2D texture = GetTextureFromRandomMap(randomMap);
        _XYDisplayRenderer.sharedMaterial.mainTexture = texture;

        _XYAverageText.text = GetAverage(randomMap).ToString();
    }

    void DisplayYZRandomMap()
    {
        float[,] randomMap = GetYZRandomMap(_length, _length, _offset);
        Texture2D texture = GetTextureFromRandomMap(randomMap);
        _YZDisplayRenderer.sharedMaterial.mainTexture = texture;

        _YZAverageText.text = GetAverage(randomMap).ToString();
    }

    void DisplayXZRandomMap()
    {
        float[,] randomMap = GetXZRandomMap(_length, _length, _offset);
        Texture2D texture = GetTextureFromRandomMap(randomMap);
        _XZDisplayRenderer.sharedMaterial.mainTexture = texture;

        _XZAverageText.text = GetAverage(randomMap).ToString();
    }



    float[,] GetXYRandomMap(int width, int height, Vector3Int offset)
    {
        float[,] map = new float[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = RandomNumber.GetFloat(x + offset.x, y + offset.y, _offset.z);

        return map;
    }

    float[,] GetYZRandomMap(int width, int height, Vector3Int offset)
    {
        float[,] map = new float[width, height];

        for (int y = 0; y < width; y++)
            for (int z = 0; z < height; z++)
                map[y, z] = RandomNumber.GetFloat(_offset.x, y + offset.y, z + offset.z);

        return map;
    }

    float[,] GetXZRandomMap(int width, int height, Vector3Int offset)
    {
        float[,] map = new float[width, height];

        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
                map[x, z] = RandomNumber.GetFloat(x + offset.x, _offset.y, z + offset.z);

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
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }


    float GetAverage(float[,] map)
    {
        float average = 0;

        foreach (float perlinValue in map)
            average += perlinValue;

        return average / (map.GetLength(0) * map.GetLength(1));
    }


    private void OnValidate()
    {
        if (_length < 1) _length = 1;

        DisplayRandomMap();
    }
}
