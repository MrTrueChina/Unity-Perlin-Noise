using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OneDimensionalPerlinNoiseGenerator
{
    //octaves：倍频    persistence：持续度    lacunarity：隙度
    public static float GetPerlinValue(int seed, float xPos, float scale, int octaves, float persistence, float lacunarity)
    {
        float amplitude = 1;        //振幅
        float frequency = 1;        //频率

        return 0;
    }

    /*
     *  重点在于柏林噪声的连续性，既是连续的同时又是不重复的，至少不能明显看出来重复
     *  
     *  需要考虑偏移和缩放，先缩放后偏移
     */

    public static float GetSinglePerlinValue(int seed, float originPos, float scale)
    {
        float xPos = originPos * scale;

        return GetSinglePerlinValue(seed, xPos);
    }

    /*
     *  先解决单个值的
     *  之后是单个线的
     *  之后是多个线的
     */

    //public static float[] GetAPerlinLine(int seed, )

    public static float GetSinglePerlinValue(int seed, float xPos)
    {
        int leftPos = Mathf.FloorToInt(xPos);                   //Mathf.FlooarToInt(float f)：向下找最近的int，如果float就是个int值则返回这个int
        int rightPos = leftPos + 1;

        float leftValue = RandomNumber.GetFloat(seed, leftPos);
        float rightValue = RandomNumber.GetFloat(seed, rightPos);

        float rate = xPos - leftPos;                            //算比例，柏林噪声的值绝大部分情况是位于两个随机数之间，需要算比例得出结果
        rate = 1 - (Mathf.Cos(rate * Mathf.PI) / 2 + 0.5f);     //这一步是平滑

        return Mathf.Lerp(leftValue, rightValue, rate);
    }
}
