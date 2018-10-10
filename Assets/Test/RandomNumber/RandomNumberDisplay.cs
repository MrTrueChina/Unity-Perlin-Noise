using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumberDisplay : MonoBehaviour
{
    [SerializeField]
    int _seed;
    [SerializeField]
    int _length = 100;
    [SerializeField]
    float _scale = 10;
    [SerializeField]
    int _offset;

    [SerializeField]
    bool _drawSingleSeed = true;
    [SerializeField]
    bool _drawSeedAndX = true;


    private void OnDrawGizmos()
    {
        DrawHalfLine();

        if(_drawSingleSeed) DrawSingleSeed();
        if(_drawSeedAndX) DrawSeedAngX();
    }

    void DrawHalfLine()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, 0.5f, 0), new Vector3(_length / _scale, 0.5f, 0));
    }

    void DrawSingleSeed()
    {
        Gizmos.color = Color.gray;

        float num = 0;

        for (int i = 0; i < _length; i++)
        {
            float randomValue = RandomNumber.GetFloat(_seed + i);
            Vector3 position = new Vector3(i / _scale, randomValue, 0);
            Gizmos.DrawSphere(position, 0.01f);

            num += randomValue;
        }

        num /= _length;
        Gizmos.DrawLine(new Vector3(0, num, 0), new Vector3(_length / _scale, num, 0));
    }

    void DrawSeedAngX()
    {
        Gizmos.color = Color.green;

        float num = 0;

        for (int x = 0; x < _length; x++)
        {
            float randomValue = RandomNumber.GetFloat(_seed, x + _offset);
            Vector3 position = new Vector3(x / _scale, randomValue, 0);
            Gizmos.DrawSphere(position, 0.01f);

            num += randomValue;
        }

        num /= _length;
        Gizmos.DrawLine(new Vector3(0, num, 0), new Vector3(_length / _scale, num, 0));
    }



    private void OnValidate()
    {
        if (_length > 1000) _length = 1000;
        if (_scale < 0.01f) _scale = 0.01f;
    }
}
