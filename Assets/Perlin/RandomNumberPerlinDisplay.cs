using UnityEngine;

public class RandomNumberPerlinDisplay : MonoBehaviour
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
    float _seed;
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
    Vector2 _offset;

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
        float[,] noiseMap = new float[_sideLength, _sideLength];
        for (int x = 0; x < _sideLength; x++)
            for (int y = 0; y < _sideLength; y++)
                noiseMap[x, y] = RandomNumberPerlinNoise.GetPerlinValue(x / _scale + _offset.x, y / _scale + _offset.y, _seed + _speed * time, _octaves, _persistence, _lacunarity);
                //noiseMap[x, y] = PerlinRandom.GetInt((int)(x / _scale + _offset.x), (int)(y / _scale + _offset.y), (int)(_seed + _speed * time), 0, 8) / 7f;

        Color[] colorMap = new Color[_sideLength * _sideLength];
        for (int x = 0; x < _sideLength; x++)
            for (int y = 0; y < _sideLength; y++)
                colorMap[y * _sideLength + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);

        Texture2D texture = new Texture2D(_sideLength, _sideLength);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = _filterMode;
        texture.SetPixels(colorMap);
        texture.Apply();

        renderer.sharedMaterial.mainTexture = texture;

        //float a = 0;
        //foreach (float f in noiseMap)
        //    a += f;
        //Debug.Log(a / _sideLength / _sideLength);
    }


    private void OnValidate()
    {
        if (_sideLength < 1) _sideLength = 1;
        if (_scale < 0.01f) _scale = 0.01f;

        if (renderer != null)
            Genarete();
    }
}
