using UnityEngine;
using UnityEngine.UI;

public class SingleParameterRandomNumberDisplay : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;
    [SerializeField]
    Text _averageText;

    [SerializeField]
    int _startSeed;         //测试随机数生成方法当然不能只测试一个数，这个值是最开始的种子，后续的种子是不断++出来的
    [SerializeField]
    int _length = 100;      //总共生成的随机数数量
    [SerializeField]
    int _step = 1;         //每一步前进几个种子


    public void Generate()
    {
        System.Random random = new System.Random(_startSeed);
        float[,] randomMap = new float[_length, _length];
        for (int x = 0; x < _length; x++)
            for (int y = 0; y < _length; y++)
                randomMap[x, y] = (RandomNumber.GetInt(x + y * _length) * _step + _startSeed) % 2;
                //randomMap[x, y] = random.Next() % 2 / 1f;

        Color[] colorMap = new Color[_length * _length];
        for (int x = 0; x < _length; x++)
            for (int y = 0; y < _length; y++)
                colorMap[x + y * _length] = Color.Lerp(Color.black, Color.white, randomMap[x, y]);

        Texture2D texture = new Texture2D(_length, _length);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        _renderer.sharedMaterial.mainTexture = texture;

        float average = 0;
        foreach (float value in randomMap)
            average += value;
        _averageText.text = (average / _length / _length).ToString();
    }


    private void OnValidate()
    {
        _length = Mathf.Clamp(_length, 1, 1000);

        if (_renderer != null && _averageText != null)
            Generate();
    }
}
