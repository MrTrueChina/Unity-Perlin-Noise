/*
 *  三维随机数法柏林噪声
 *  
 *  极端优化，降低可读性换取效率
 *  
 *  详细原理请看
 *  中文版：https://www.cnblogs.com/leoin2012/p/7218033.html
 *  English：http://flafla2.github.io/2014/08/09/perlinnoise.html
 */

namespace MtC.Tools.PerlinNoise
{
    public static class RandomPerlinNoise
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
            int intgerX = x >= 0 ? (int)x : (int)x - 1;     //内联 Floor
            int intgerY = y >= 0 ? (int)y : (int)y - 1;
            int intgerZ = z >= 0 ? (int)z : (int)z - 1;

            //取小数，确认在随机数区域内的位置
            float decimalX = x - intgerX;
            float decimalY = y - intgerY;
            float decimalZ = z - intgerZ;

            //计算平滑比例，在这里算一次之后在后面只要用Lerp就能通过比例直接计算出影响值，可以解决计算量
            float xProportion = decimalX * decimalX * decimalX * (decimalX * (decimalX * 6 - 15) + 10);     //低成本轻量平滑，曲线看起来像是幅度减缓版的cos平滑    cos平滑太集中，线性平滑太分散，都很容易被看出来随机数点
            float yProportion = decimalY * decimalY * decimalY * (decimalY * (decimalY * 6 - 15) + 10);     //公式来自 https://gist.github.com/Flafla2/f0260a861be0ebdeef76
            float zProportion = decimalZ * decimalZ * decimalZ * (decimalZ * (decimalZ * 6 - 15) + 10);


            //正式计算
            float xyzValue = Lerp
            (
                Lerp
                (
                    Lerp
                    (
                        Gradient(intgerX    , intgerY    , intgerZ    , decimalX    , decimalY    , decimalZ    ),  //坐标(0,0,0)的点的影响值
                        Gradient(intgerX + 1, intgerY    , intgerZ    , decimalX - 1, decimalY    , decimalZ    ),  //坐标(1,0,0)的点的影响值，decimalX之所以是负的是因为这个点到取值点的z轴方向是朝向负方向的
                        xProportion                                                                                 //坐标(x,0,0)的边的影响值
                    ),
                    Lerp
                    (
                        Gradient(intgerX    , intgerY + 1, intgerZ    , decimalX    , decimalY - 1, decimalZ    ),  //坐标(0,1,0)的点的影响值
                        Gradient(intgerX + 1, intgerY + 1, intgerZ    , decimalX - 1, decimalY - 1, decimalZ    ),  //坐标(1,1,0)的点的影响值
                        xProportion                                                                                 //坐标(x,1,0)的边的影响值
                    ), 
                    yProportion                                                                                     //坐标(x,y,0)的面的影响值
                ),
                Lerp
                (
                    Lerp
                    (
                        Gradient(intgerX    , intgerY    , intgerZ + 1, decimalX    , decimalY    , decimalZ - 1),  //坐标(0,0,1)的点的影响值
                        Gradient(intgerX + 1, intgerY    , intgerZ + 1, decimalX - 1, decimalY    , decimalZ - 1),  //坐标(1,0,1)的点的影响值
                        xProportion                                                                                 //坐标(x,0,1)的边的影响值
                    ),
                    Lerp
                    (
                        Gradient(intgerX    , intgerY + 1, intgerZ + 1, decimalX    , decimalY - 1, decimalZ - 1),  //坐标(0,1,1)的点的影响值
                        Gradient(intgerX + 1, intgerY + 1, intgerZ + 1, decimalX - 1, decimalY - 1, decimalZ - 1),  //坐标(1,1,1)的点的影响值
                        xProportion                                                                                 //坐标(x,1,1)的边的影响值
                    ), 
                    yProportion                                                                                     //坐标(x,y,1)的面的影响值
                ),
                zProportion                                                                                         //坐标(x,y,z)的立方体的总影响值
            );

            return (xyzValue + 1) / 2;  //最终控制到正数范围，用half算法解决
        }

        //计算一个顶点对最终柏林噪声值的影响值，前三个参数是这个订单的坐标，后三个参数是顶点到柏林噪声取值点的距离向量
        static float Gradient(int cornerX, int cornerY, int cornerZ, float x, float y, float z)
        {
            switch 
            (
                (int)(8 * (float)((cornerX + 1111111111) * (cornerY - 333333333) * (cornerZ + 555555555) * (cornerX + cornerY + 777777777) * (cornerX + cornerZ - 999999999) * (cornerZ - cornerY + 233333333) & 2147483646) / int.MaxValue)
            )
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
            //不同的随机角度运行速度、占用资源、最终图像都不尽相同，如果想要获取更好的效果可以尝试修改随机向量
        }

        static float Lerp(float a, float b, float t)    //Lerp也是为了节约计算量手写一个
        {
            return (b - a) * t + a;
        }
    }
}