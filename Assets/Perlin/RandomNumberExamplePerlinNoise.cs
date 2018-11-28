/*
 *  三维随机数法柏林噪声-算法展示版
 *  
 *  用随机数发生器计算每个订单的向量，相对于查表法优点是占用内存少、几乎无限不重复，缺点是速度慢
 *  
 *  详细原理请看
 *  中文版：https://www.cnblogs.com/leoin2012/p/7218033.html
 *  English：http://flafla2.github.io/2014/08/09/perlinnoise.html
 */

public static class RandomNumberExamplePerlinNoise
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


    static float Perlin(float x, float y, float z)
    {
        //取整数，确认在哪个随机数区域里
        int intgerX = Floor(x);
        int intgerY = Floor(y);
        int intgerZ = Floor(z);

        //取小数，确认在随机数区域内的位置
        float decimalX = x - intgerX;
        float decimalY = y - intgerY;
        float decimalZ = z - intgerZ;

        //计算平滑比例，在这里算一次之后在后面只要用Lerp就能通过比例直接计算出影响值，可以解决计算量
        float xProportion = Fade(decimalX);
        float yProportion = Fade(decimalY);
        float zProportion = Fade(decimalZ);


        //正式计算
        float oooValue = Gradient(intgerX    , intgerY    , intgerZ    , decimalX    , decimalY    , decimalZ    );    //坐标(0,0,0)的点的影响值
        float ooiValue = Gradient(intgerX    , intgerY    , intgerZ + 1, decimalX    , decimalY    , decimalZ - 1);    //坐标(0,0,1)的点的影响值，decimalZ之所以是负的是因为这个点到取值点的z轴方向是朝向负方向的
        float oioValue = Gradient(intgerX    , intgerY + 1, intgerZ    , decimalX    , decimalY - 1, decimalZ    );    //坐标(0,1,0)的点的影响值
        float oiiValue = Gradient(intgerX    , intgerY + 1, intgerZ + 1, decimalX    , decimalY - 1, decimalZ - 1);    //坐标(0,1,1)的点的影响值
        float iooValue = Gradient(intgerX + 1, intgerY    , intgerZ    , decimalX - 1, decimalY    , decimalZ    );    //坐标(1,0,0)的点的影响值
        float ioiValue = Gradient(intgerX + 1, intgerY    , intgerZ + 1, decimalX - 1, decimalY    , decimalZ - 1);    //坐标(1,0,1)的点的影响值
        float iioValue = Gradient(intgerX + 1, intgerY + 1, intgerZ    , decimalX - 1, decimalY - 1, decimalZ    );    //坐标(1,1,0)的点的影响值
        float iiiValue = Gradient(intgerX + 1, intgerY + 1, intgerZ + 1, decimalX - 1, decimalY - 1, decimalZ - 1);    //坐标(1,1,1)的点的影响值

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
            default: return 0;  //正常情况下不会执行到这一步
        }
        //完全随机角向量成本太高，这里用了从中心指向八个角的向量，喜欢的话也可以换成指向12个边的、指向6个面的，或者斜着的，只要保证总和是均匀指向周围的就行
    }

    static int GetRandomInt(int x, int y, int z, int min, int max)
    {
        int value = (x + 1111111111) * (y - 333333333) * (z + 555555555) * (x + y + 777777777) * (x + z - 999999999) * (z - y + 233333333); //数写多少不重要，重要的是【够大】，要【看起来无规律的】【均匀的】按照xyz算出int值
        value = value & int.MaxValue - 1;       //用与运算限制范围在 0 - (int.MaxValue - 1)，-1是因为如果这一步达到了int最大值下一步就会返回最大值参数，设计上是不应该返回最大值的
        return (int)((max - min) * (float)value / int.MaxValue + min);  //将随机数等比例缩小到参数设置的范围，之后返回
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
