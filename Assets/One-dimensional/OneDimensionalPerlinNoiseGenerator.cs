using UnityEngine;

public static class OneDimensionalPerlinNoiseGenerator
{
    /*
     *  octaves：倍频，就是多少个柏林噪声混合出最终噪声
     *  persistence：持续度，后续噪声对最终噪声的效果影响，持续度越大后续噪声的影响就越大，应该是在0-1，超过1会导致噪声最大值超出1，小于0则会降低噪声甚至降到负数
     *  lacunarity：隙度，可能是和缩放类似的值，这个值影响到后续噪声的缩放值，越靠后影响越大
     */
    public static float GetPerlinValue(int seed, float originPos, float scale, int octaves, float persistence, float lacunarity)
    {
        persistence = Mathf.Clamp(persistence, 0, 1);

        float perlinValue = 0;
        float totalAmplitude = 0;
        for (int i = 0; i < octaves; i++)
        {
            int currentSeed = RandomNumber.GetInt(seed, i);

            float amplitude = Mathf.Pow(persistence, i);    //当前循环的振幅，或者说当前循环代表的噪声的振幅
            totalAmplitude += amplitude;                    //保留下这个振幅，最后要用来把噪声限制到 0-1

            float currentFrequency = Mathf.Pow(lacunarity, i);  //当前噪声的频率，应该就是一个缩放值吧

            float currentPerlinValue = GetSinglePerlinValue(currentSeed, originPos, scale * currentFrequency) * amplitude;     //把频率和缩放一起丢进去看看结果
            perlinValue += currentPerlinValue;
        }

        perlinValue /= totalAmplitude;

        return perlinValue;
    }


    public static float GetSubPerlinValue(int seed, float originPos, float scale, float persistence, float lacunarity, int index)
    {
        persistence = Mathf.Clamp(persistence, 0, 1);

        seed = RandomNumber.GetInt(seed, index);

        float amplitude = Mathf.Pow(persistence, index);    //当前循环的振幅，或者说当前循环代表的噪声的振幅

        float currentFrequency = Mathf.Pow(lacunarity, index);  //当前噪声的频率，应该就是一个缩放值吧

        float perlinValue = GetSinglePerlinValue(seed, originPos, scale * currentFrequency) * amplitude;     //把频率和缩放一起丢进去看看结果

        return perlinValue;
    }

    /*
     *  重点在于柏林噪声的连续性，既是连续的同时又是不重复的，至少不能明显看出来重复
     *  
     *  需要考虑偏移和缩放，先缩放后偏移
     *  
     *  看图像是连续的了
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
