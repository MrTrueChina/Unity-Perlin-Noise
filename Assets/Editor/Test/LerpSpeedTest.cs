using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class LerpSpeedTest
{
    int _loopTime = 10000000;
    float _a = 100;
    float _b = 200;
    float _t = 0.314159265f;

    float _container;


    [Test]
    public void LerpSpeed()
    {
        Debug.Log("Loop " + _loopTime + " time");
        Debug.Log("Mathf.Lerp      |" + GetMethodTime(MathfLerp) + "  _container = " + _container);
        Debug.Log("Lerp method   |" + GetMethodTime(LerpMethod) + "  _container = " + _container);
        Debug.Log("inline Lerp      |" + GetMethodTime(InlineLerp) + "  _container = " + _container);
        Debug.Log("很明显，Mathf.Lerp最慢，自写Lerp方法速度大约是Mathf.Lerp的两倍，而内联的速度最快，约为Mathf.Lerp的5倍");
        Debug.Log("带来速度提升的并不是算法，而是节省了调用的时间");
    }

    void MathfLerp()
    {
        for (int i = 0; i < _loopTime; i++)
            _container = Mathf.Lerp(_a, _b, _t);
    }

    void LerpMethod()
    {
        for (int i = 0; i < _loopTime; i++)
            _container = Lerp(_a, _b, _t);
    }
    float Lerp(float a, float b, float t)
    {
        return (b - a) * t + a;
    }

    void InlineLerp()
    {
        for (int i = 0; i < _loopTime; i++)
            _container = (_b - _a) * _t + _a;
    }


    long GetMethodTime(Action method)
    {
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        method();
        timer.Stop();
        return timer.ElapsedMilliseconds;
    }
}
