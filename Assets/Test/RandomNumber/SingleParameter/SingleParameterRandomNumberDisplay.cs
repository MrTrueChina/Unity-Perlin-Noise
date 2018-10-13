/*
 *  用来显示单参数随机数生成方法的显示脚本，主要拿来测试算法了（如果乱算数也是算法的话）
 */

/*
 *  给随机数发生器的新朋友
 */


using UnityEngine;
using UnityEngine.UI;

public class SingleParameterRandomNumberDisplay : MonoBehaviour
{
    [SerializeField]
    Text _averageText;

    [SerializeField]
    int _startSeed;         //测试随机数生成方法当然不能只测试一个数，这个值是最开始的种子，后续的种子是不断++出来的
    [SerializeField]
    int _length = 100;      //总共生成的随机数数量
    [SerializeField]
    float _scale = 10;      //这个是为了方便观察


    private void OnDrawGizmos()
    {
        DrawHalfLine();

        DrawSingleSeed();
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
            float randomValue = RandomNumber.GetFloat(_startSeed + i);
            Vector3 position = new Vector3(i / _scale, randomValue, 0);
            Gizmos.DrawSphere(position, 0.01f);

            num += randomValue;
        }

        num /= _length;
        Gizmos.DrawLine(new Vector3(0, num, 0), new Vector3(_length / _scale, num, 0));
        _averageText.text = num.ToString();
    }



    private void OnValidate()
    {
        if (_length > 1000) _length = 1000;
        if (_scale < 0.01f) _scale = 0.01f;
    }
}
