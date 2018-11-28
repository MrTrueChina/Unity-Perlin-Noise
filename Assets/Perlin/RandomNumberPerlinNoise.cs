using UnityEngine;

public static class RandomNumberPerlinNoise
{
    //octaves：倍频，就是多少个柏林噪声混合出最终噪声
    //persistence：持续度，后续噪声对最终噪声的效果影响，持续度越大后续噪声的影响就越大，应该是在0-1，超过1会导致噪声最大值超出1，小于0则会降低噪声甚至降到负数
    //lacunarity：隙度，可能是和缩放类似的值，这个值影响到后续噪声的缩放值，越靠后影响越大
    public static float GetPerlinValue(float x, float y, float z, int octaves, float persistence, float lacunarity)
    {
        float totalValue = 0;
        float maxValue = 0;
        float frequency = 1;    //frequency：频率
        float amplitude = 1;    //amplitude：振幅
        for (int i = 0; i < octaves; i++)
        {
            totalValue += Perlin(x * frequency, y * frequency, z * frequency) * amplitude;

            maxValue += amplitude;

            frequency *= lacunarity;
            amplitude *= persistence;
        }

        return totalValue / maxValue;
    }


    public static float Perlin(float x, float y, float z)
    {
        //确认整数位置
        int intgerX = (int)Mathf.Floor(x);
        int intgerY = (int)Mathf.Floor(y);
        int intgerZ = (int)Mathf.Floor(z);

        //确认小数位置
        float decimalX = x - Mathf.Floor(x);
        float decimalY = y - Mathf.Floor(y);
        float decimalZ = z - Mathf.Floor(z);

        //计算平滑比例
        float xProportion = Fade(decimalX);
        float yProportion = Fade(decimalY);
        float zProportion = Fade(decimalZ);


        //正式计算
        float oooValue = Gradient(intgerX    , intgerY    , intgerZ    , decimalX    , decimalY    , decimalZ    );    //坐标(0,0,0)的点的影响值
        float ooiValue = Gradient(intgerX    , intgerY    , intgerZ + 1, decimalX    , decimalY    , decimalZ - 1);    //坐标(0,0,1)的点的影响值，z之所以是负的是因为这个点到取值点的x轴方向是朝向负方向的
        float oioValue = Gradient(intgerX    , intgerY + 1, intgerZ    , decimalX    , decimalY - 1, decimalZ    );    //坐标(0,1,0)的点的影响值
        float oiiValue = Gradient(intgerX    , intgerY + 1, intgerZ + 1, decimalX    , decimalY - 1, decimalZ - 1);    //坐标(0,1,1)的点的影响值
        float iooValue = Gradient(intgerX + 1, intgerY    , intgerZ    , decimalX - 1, decimalY    , decimalZ    );    //坐标(1,0,0)的点的影响值
        float ioiValue = Gradient(intgerX + 1, intgerY    , intgerZ + 1, decimalX - 1, decimalY    , decimalZ - 1);    //坐标(1,0,1)的点的影响值
        float iioValue = Gradient(intgerX + 1, intgerY + 1, intgerZ    , decimalX - 1, decimalY - 1, decimalZ    );    //坐标(1,1,0)的点的影响值
        float iiiValue = Gradient(intgerX + 1, intgerY + 1, intgerZ + 1, decimalX - 1, decimalY - 1, decimalZ - 1);    //坐标(1,1,1)的点的影响值

        float xooValue = Mathf.Lerp(oooValue, iooValue, xProportion);   //坐标为(x,0,0)的边上的影响值
        float xoiValue = Mathf.Lerp(ooiValue, ioiValue, xProportion);   //(x,0,1)的边
        float xioValue = Mathf.Lerp(oioValue, iioValue, xProportion);   //(x,1,0)的边
        float xiiValue = Mathf.Lerp(oiiValue, iiiValue, xProportion);   //(x,1,1)的边

        float xyoValue = Mathf.Lerp(xooValue, xioValue, yProportion);   //坐标为(x,y,0)面上是影响值
        float xyiValue = Mathf.Lerp(xoiValue, xiiValue, yProportion);   //(x,y,1)的面

        float xyzValue = Mathf.Lerp(xyoValue, xyiValue, zProportion);   //坐标为(x,y,z)的立方体的影响值，也就是三维柏林噪声的最终值

        float perlinValue = (xyzValue + 1) / 2; //最终控制到正数范围，用half算法解决

        return perlinValue;
    }

    static float Gradient(int cornerX,int cornerY,int cornerZ, float x, float y, float z)
    {
        switch (GetRandomInt(cornerX, cornerY, cornerZ, 0, 8))     //根据点坐标随机出从中心到八个角的八个向量，并用这些向量和后三个参数组成的距离向量求点积，注意随机数发生器不会返回最大值，所以要用 0,8
        {
            case 0: return  x + y + z;  //( 1, 1, 1)
            case 1: return  x + y - z;  //( 1, 1,-1)
            case 2: return  x - y + z;  //( 1,-1, 1)
            case 3: return  x - y - z;  //( 1,-1,-1)
            case 4: return -x + y + z;  //(-1, 1, 1)
            case 5: return -x + y - z;  //(-1, 1,-1)
            case 6: return -x - y + z;  //(-1,-1, 1)
            case 7: return -x - y - z;  //(-1,-1,-1)
            default:
                Debug.LogError("PerlinNoise Default! Check RandomNumber  PerlinRandom.GetInt(cornerX, cornerY, cornerZ, 0, 8) = " + PerlinRandom.GetInt(cornerX, cornerY, cornerZ, 0, 8));  //正常情况下不会到这一步，如果真的发生了，应该是随机数发生器返回了错误值，最有可能是返回了负数，其次是返回了8
                return 0;
        }
    }

    static int GetRandomInt(int x, int y, int z, int min, int max)
    {
        int value = (x + 1111111111) * (y - 333333333) * (z + 555555555) * (x + y + 777777777) * (x + z - 999999999) * (z - y + 233333333); //数写多少不重要，重要的是 看起来无规律的、比较平均的 按照xyz算出int值
        value = value & int.MaxValue - 1;
        return (int)((max - min) * (float)value / int.MaxValue + min);
    }

    static float Fade(float f)
    {
        return f * f * f * (f * (f * 6 - 15) + 10); //低成本轻量平滑
        //公式来自 https://gist.github.com/Flafla2/f0260a861be0ebdeef76
    }
}
