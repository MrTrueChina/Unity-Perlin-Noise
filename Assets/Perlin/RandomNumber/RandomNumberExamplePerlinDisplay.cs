using UnityEngine;

public class RandomNumberExamplePerlinDisplay : MonoBehaviour
{
    new Renderer renderer
    {
        get
        {
            if (_renderer != null)
                return _renderer;

            _renderer = GetComponent<Renderer>();
            return _renderer;
        }
    }
    Renderer _renderer;
    
    [SerializeField]
    int _sideLength;
    [SerializeField]
    [Range(1, 7)]
    int _octaves;
    [SerializeField]
    float _persistence;
    [SerializeField]
    float _lacunarity;
    [SerializeField]
    float _scale;
    [SerializeField]
    Vector3 _offset;

    [SerializeField]
    FilterMode _filterMode;

    [SerializeField]
    float _speed;



    private void Update()
    {
        Genarete(Time.time);
    }


    public void Genarete(float time = 0)
    {
        float[,] noiseMap = GetNoiseMap(time);
        Color[] colorMap = GetColorMap(noiseMap);
        Texture2D texture = CreatTexture(colorMap);

        renderer.sharedMaterial.mainTexture = texture;
    }

    float[,] GetNoiseMap(float time)
    {
        float[,] noiseMap = new float[_sideLength, _sideLength];
        for (int x = 0; x < _sideLength; x++)
            for (int y = 0; y < _sideLength; y++)
                noiseMap[x, y] = RandomNumberExamplePerlinNoise.GetPerlinValue(x / _scale + _offset.x, y / _scale + _offset.y, _offset.z + _speed * time, _octaves, _persistence, _lacunarity);

        return noiseMap;
    }

    Color[] GetColorMap(float[,] noiseMap)
    {
        Color[] colorMap = new Color[_sideLength * _sideLength];
        for (int x = 0; x < _sideLength; x++)
            for (int y = 0; y < _sideLength; y++)
                colorMap[y * _sideLength + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);

        return colorMap;
    }

    Texture2D CreatTexture(Color[] colorMap)
    {
        Texture2D texture = new Texture2D(_sideLength, _sideLength);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = _filterMode;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }



    private void OnValidate()
    {
        if (_sideLength < 1) _sideLength = 1;
        if (_scale < 0.01f) _scale = 0.01f;
        _octaves = Mathf.Clamp(_octaves, 1, 7);

        if (renderer != null)
            Genarete();
    }
}
