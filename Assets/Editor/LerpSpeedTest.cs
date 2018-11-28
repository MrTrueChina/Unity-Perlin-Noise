using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class LerpSpeedTest
{
    [Test]
    public void LerpSpeed()
    {
        int loopTime = 10000000;
        float f;
        float min = 100;
        float max = 200;
        float proportion = 0.314159265f;


        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        timer.Start();
        for (int i = 0; i < loopTime; i++)
        {
            f = Mathf.Lerp(min, max, proportion);
        }
        timer.Stop();
        Debug.Log("Mathf.Lerp：" + timer.ElapsedMilliseconds);


        timer.Reset();
        timer.Start();
        for (int i = 0; i < loopTime; i++)
            f = Lerp(min, max, proportion);
        timer.Stop();
        Debug.Log(timer.ElapsedMilliseconds);
    }

    float Lerp(float min, float max, float proportion)
    {
        return (max - min) * proportion + min;
    }
}
