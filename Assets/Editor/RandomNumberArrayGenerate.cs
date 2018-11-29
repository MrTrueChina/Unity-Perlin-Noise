/*
 *  随机数列生成器，在 Test Runner 面板执行后会输出 0-_length-1 的不重复随机数列
 */

using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class RandomNumberArrayGenerate
{
    int _length = 64;

    [Test]
    public void Generate()
    {
        List<int> origin = new List<int>();
        for (int i = 0; i < _length; i++)
            origin.Add(i);

        int[] randomNumberArray = new int[_length];
        System.Random random = new System.Random();
        for (int i = 0; i < _length; i++)
        {
            int index = random.Next(0, origin.Count);
            randomNumberArray[i] = origin[index];
            origin.RemoveAt(index);
        }

        string randomNumberString = "";
        foreach (int i in randomNumberArray)
            randomNumberString += i + ",";

        randomNumberString = randomNumberString.Substring(0, randomNumberString.Length - 1);

        Debug.Log(randomNumberString);
    }
}
