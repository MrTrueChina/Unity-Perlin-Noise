/*
 *  三维查表法柏林噪声-算法展示版
 *  
 *  通过查找预先准备的真随机数表确定每个订单的向量，相对于随机数法优点是速度极快，缺点是占用大量内存、会随着查找表循环
 *  
 *  详细原理请看
 *  中文版：https://www.cnblogs.com/leoin2012/p/7218033.html
 *  English：http://flafla2.github.io/2014/08/09/perlinnoise.html
 */

public static class ArrayExamplePerlinNoise
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

    //预先准备一个随机数组，长度至少是柏林噪声循环一次的随机数的数量的两倍，并且最小值是0，最大值是一次循环的随机数总数，各个数值不重复，这个数组本身也要按照最大循环次数为一个循环进行循环
    static readonly int[] RN = new int[128] {
        11,57,62,30,56,28,53,39,32,38,26,51,22,18,47,8,43,42,1,50,16,55,48,3,29,58,6,45,10,20,37,25,9,27,15,54,19,14,52,61,59,36,21,24,2,33,35,17,5,31,44,23,40,46,41,49,34,12,4,0,7,60,63,13,
        11,57,62,30,56,28,53,39,32,38,26,51,22,18,47,8,43,42,1,50,16,55,48,3,29,58,6,45,10,20,37,25,9,27,15,54,19,14,52,61,59,36,21,24,2,33,35,17,5,31,44,23,40,46,41,49,34,12,4,0,7,60,63,13
    };
    //这个柏林噪声以64个随机数为一个循环，所以随机数组是0-63的唯一随机数重复两次

 
    static float Perlin(float x, float y, float z)
    {
        //UnityEngine.Debug.Log("y = " + y);
        //if (y < 0)
            //UnityEngine.Debug.Log("-y % 64 = " + (-y % 64));
            //UnityEngine.Debug.Log("64 - (-y % 64) = " + (64 - (-y % 64)));
            //UnityEngine.Debug.Log("(64 - (-y % 64)) % 64 = " + ((64 - (-y % 64)) % 64));
        //转化坐标到随机数循环长度内，这个柏林噪声是 0-64
        //  0的时候是0  ->  if(x == 0)  x = x
        // 64的时候是0  ->  if(x > 0)   x = x % 64
        //-64的时候是0  ->  if(x < 0)   x = -x % 64
        //不能镜像      ->  if(x < 0)   x = (64 - (-x % 64)) % 64
        x = x >= 0 ? x % 64 : (64 - (-x % 64)) % 64;
        if (x == 64) x = 0;
        y = y >= 0 ? y % 64 : 64 - (-y % 64);
        if (y == 64) y = 0;
        z = z >= 0 ? z % 64 : (64 - (-z % 64)) % 64;
        if (z == 64) z = 0;
        //UnityEngine.Debug.Log("y = " + y);
        //UnityEngine.Debug.Log("z = " + z);

        //取整数，确认在哪个随机数区域里
        int intgerX = Floor(x);
        int intgerY = Floor(y);
        int intgerZ = Floor(z);
        //UnityEngine.Debug.Log("intgerX = " + intgerX);
        //UnityEngine.Debug.Log("intgerY = " + intgerY);

        //取小数，确认在随机数区域内的位置
        float decimalX = x - intgerX;
        float decimalY = y - intgerY;
        float decimalZ = z - intgerZ;

        //计算平滑比例，在这里算一次之后在后面只要用Lerp就能通过比例直接计算出影响值，可以解决计算量
        float xProportion = Fade(decimalX);
        float yProportion = Fade(decimalY);
        float zProportion = Fade(decimalZ);


        //正式计算
        //UnityEngine.Debug.Log(RN[0]);
        float oooValue = Gradient(RN[RN[RN[intgerX    ] + intgerY    ] + intgerZ    ], decimalX    , decimalY    , decimalZ    );   //坐标(0,0,0)的点的影响值
        float ooiValue = Gradient(RN[RN[RN[intgerX    ] + intgerY    ] + intgerZ + 1], decimalX    , decimalY    , decimalZ - 1);   //坐标(0,0,1)的点的影响值，decimalZ之所以是负的是因为这个点到取值点的z轴方向是朝向负方向的
        float oioValue = Gradient(RN[RN[RN[intgerX    ] + intgerY + 1] + intgerZ    ], decimalX    , decimalY - 1, decimalZ    );   //坐标(0,1,0)的点的影响值
        float oiiValue = Gradient(RN[RN[RN[intgerX    ] + intgerY + 1] + intgerZ + 1], decimalX    , decimalY - 1, decimalZ - 1);   //坐标(0,1,1)的点的影响值
        float iooValue = Gradient(RN[RN[RN[intgerX + 1] + intgerY    ] + intgerZ    ], decimalX - 1, decimalY    , decimalZ    );   //坐标(1,0,0)的点的影响值
        float ioiValue = Gradient(RN[RN[RN[intgerX + 1] + intgerY    ] + intgerZ + 1], decimalX - 1, decimalY    , decimalZ - 1);   //坐标(1,0,1)的点的影响值
        float iioValue = Gradient(RN[RN[RN[intgerX + 1] + intgerY + 1] + intgerZ    ], decimalX - 1, decimalY - 1, decimalZ    );   //坐标(1,1,0)的点的影响值
        float iiiValue = Gradient(RN[RN[RN[intgerX + 1] + intgerY + 1] + intgerZ + 1], decimalX - 1, decimalY - 1, decimalZ - 1);   //坐标(1,1,1)的点的影响值

        float xooValue = Lerp(oooValue, iooValue, xProportion);   //坐标为(x,0,0)的边上的影响值
        float xoiValue = Lerp(ooiValue, ioiValue, xProportion);   //(x,0,1)的边
        float xioValue = Lerp(oioValue, iioValue, xProportion);   //(x,1,0)的边
        float xiiValue = Lerp(oiiValue, iiiValue, xProportion);   //(x,1,1)的边

        float xyoValue = Lerp(xooValue, xioValue, yProportion);   //坐标为(x,y,0)面上是影响值
        float xyiValue = Lerp(xoiValue, xiiValue, yProportion);   //(x,y,1)的面

        float xyzValue = Lerp(xyoValue, xyiValue, zProportion);   //坐标为(x,y,z)的立方体的影响值，也就是三维柏林噪声的总值

        float perlinValue = (xyzValue + 1) / 2; //最终控制到正数范围，用half算法解决

        return perlinValue;
    }

    //计算一个顶点对最终柏林噪声值的影响值，前三个参数是这个订单的坐标，后三个参数是顶点到柏林噪声取值点的距离向量
    static float Gradient(int randomKey, float x, float y, float z)
    {
        //UnityEngine.Debug.Log(randomKey % 12);
        switch (randomKey % 12)    //根据随机数选择从中心到十二个边的十二个向量中的一个，并用这个向量和后三个参数组成的距离向量求点积，12边向量比8角向量快，噪声图形更接近网格
        {
            case 0:  return  y + z;   //( 0, 1, 1)
            case 1:  return  y - z;   //( 0, 1,-1)
            case 2:  return -y + z;   //( 0,-1, 1)
            case 3:  return -y - z;   //( 0,-1,-1)
            case 4:  return  x + z;   //( 1, 0, 1)
            case 5:  return  x - z;   //( 1, 0,-1)
            case 6:  return -x + z;   //(-1, 0, 1)
            case 7:  return -x - z;   //(-1, 0,-1)
            case 8:  return  x + y;   //( 1, 1, 0)
            case 9:  return  x - y;   //( 1,-1, 0)
            case 10: return -x + y;   //(-1, 1, 0)
            case 11: return -x - y;   //(-1,-1, 0)
            default:
                //UnityEngine.Debug.LogError(randomKey % 12);
                return 0;  //正常情况下不会执行到这一步
        }
        //完全随机角向量成本太高，这里用了从中心指向八个角的向量，喜欢的话也可以换成指向12个边的、指向6个面的，或者斜着的，只要保证总和是均匀指向周围的就行
    }

    static int Floor(float f)       //直接返回int的Floor方法，用Unity的Floor或C#的Floor也是一样效果，但那样都需要多一次调用，会多消耗计算量，柏林噪声这种高消耗算法不能做这种无意义浪费
    {
        return f >= 0 ? (int)f : (int)f - 1;
    }

    static float Lerp(float a, float b, float t)    //Lerp也是为了节约计算量手写一个
    {
        return (b - a) * t + a;
    }

    static float Fade(float f)
    {
        return f * f * f * (f * (f * 6 - 15) + 10); //低成本轻量平滑，曲线看起来像是幅度减缓版的cos平滑    cos平滑太集中，线性平滑太分散，都很容易被看出来随机数点
        //公式来自 https://gist.github.com/Flafla2/f0260a861be0ebdeef76
    }
}
