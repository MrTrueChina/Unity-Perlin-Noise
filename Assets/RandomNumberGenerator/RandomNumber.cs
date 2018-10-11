/*
 *  随机数生成器
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomNumber
{
    //单参数，是个随机数生成器但不适合柏林噪声，计算和常数随便写，只要最后看起来够随机就行，注意不要写成对称的，也就是正负seed结果相同，很容易穿帮
    public static float GetFloat(int seed)
    {
        int value = seed * 2233681 ^ seed;

        value = (value * (value - 155786) ^ seed) * (value * 1679542) + 736779151;

        return IntToZTOFloat(value);
    }

    /*
     *  种子 + x坐标，适用于1维柏林噪声的随机数发生器
     *  
     *  目标是种子更换的时候图像会发生明显变化
     *  
     *  各种常数和计算都是乱写 + 对着图像修修改改弄出来的，主要是修改
     */
    public static float GetFloat(int seed, int x)
    {
        int value = (seed * 248795 + x * 24879512 + seed * x + 157895);

        value = ((value - 2197568) * value * x) * ((x * value * 349852) * x) + 564919672;

        return IntToZTOFloat(value);
    }



    static float IntToZTOFloat(int intValue)
    {
        return (float)(intValue & int.MaxValue) / int.MaxValue;
    }
}
