/*
 *  用来显示双种子随机数方法的显示器
 */


using UnityEngine;
using UnityEngine.UI;

public class DoubleParameterRandomNumberDisplay : MonoBehaviour
{
    [SerializeField]
    Renderer _displayRenderer;
    [SerializeField]
    Text _averageText;

    [SerializeField]
    int _length;
    [SerializeField]
    Vector2Int _offset;

    [SerializeField]
    FilterMode _filterMode;
    

    /*
     *  
     */
    public void DisplayRandomMap()
    {
        float[,] randomMap = GetRandomMap(_length, _length, _offset);
        Texture2D texture = GetTextureFromRandomMap(randomMap);
        _displayRenderer.sharedMaterial.mainTexture = texture;

        _averageText.text = GetAverage(randomMap).ToString();
    }

    float[,] GetRandomMap(int width, int height, Vector2Int offset)
    {
        float[,] map = new float[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = RandomNumber.GetFloat(x + offset.x, y + offset.y);

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
        texture.filterMode = _filterMode;
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
