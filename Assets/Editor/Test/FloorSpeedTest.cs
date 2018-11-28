using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class FloorTiIntSpeedTest
{
    int _loopTime = 1000000;
    float _origin = -1.5f;

    int _container;

    [Test]
    public void FloorSpeed()
    {
        Debug.Log("Loop " + _loopTime + " Time");
        Debug.Log("(int)Mathf.Floor：             |" + GetMethodTime((f) => (int)Mathf.Floor(f)) + " | _container = " + _container);
        Debug.Log("(int)System.Math.Floor     |" + GetMethodTime((f) => (int)Math.Floor(f)) + " | _container = " + _container);
        Debug.Log("if else Floor                     |" + GetMethodTime(IfElseFloor) + " | _container = " + _container);
        Debug.Log("?: Floor                           |" + GetMethodTime(ThreeFloor) + " | _container = " + _container);
        Debug.Log("Inline if else Floor            |" + InlineIfElseFloor() + " | _container = " + _container);
        Debug.Log("Inline ?: Floor                  |" + InlineThreeFloor() + " | _container = " + _container);
        Debug.Log("可以看出，Unity的Floor最慢，C#自带的Floor稍快一点，自写的Floor方法速度比C#的Floor快至少一倍，内联方法则快了近十倍");
        Debug.Log("带来速度提升的并不是算法，而是节省了调用的时间");
    }
    

    int IfElseFloor(float f)
    {
        if (f >= 0)
            return (int)f;
        else
            return (int)f - 1;
    }


    int ThreeFloor(float f)
    {
        return f >= 0 ? (int)f : (int)f - 1;
    }


    long InlineIfElseFloor()
    {
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        for (int i = 0; i < _loopTime; i++)
            if (_origin >= 0)
                _container = (int)_origin;
            else
                _container = (int)_origin - 1;
        timer.Stop();
        return timer.ElapsedMilliseconds;
    }


    long InlineThreeFloor()
    {
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        for (int i = 0; i < _loopTime; i++)
            _container = _origin >= 0 ? (int)_origin : (int)_origin - 1;
        timer.Stop();
        return timer.ElapsedMilliseconds;
    }


    long GetMethodTime(Func<float, int> floor)
    {
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        for (int i = 0; i < _loopTime; i++)
            _container = floor(_origin);
        timer.Stop();
        return timer.ElapsedMilliseconds;
    }
}
