/*
 *  柏林噪声配对的随机数发生器
 *  
 *  传入坐标返回一个int，这个int正常情况下不会超过两位，这个发生器需要在只有两位的情况下保持随机
 *  
 *  越快越好
 */

public static class PerlinRandom
{
    public static int GetInt(int x, int y, int z, int min, int max)
    {
        int value = (x + 1111111111) * (y - 333333333) * (z + 555555555) * (x + y + 777777777) * (x + z - 999999999) * (z - y + 233333333);
        value = value & int.MaxValue - 1;
        return (int)((max - min) * (float)value / int.MaxValue + min);
    }
}
