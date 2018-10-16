using UnityEngine;

public static class TwoDimensionalPerlinNoiseGenerator
{
    /*
     *  octaves：倍频，就是多少个柏林噪声混合出最终噪声
     *  persistence：持续度，后续噪声对最终噪声的效果影响，持续度越大后续噪声的影响就越大，应该是在0-1，超过1会导致噪声最大值超出1，小于0则会降低噪声甚至降到负数
     *  lacunarity：隙度，可能是和缩放类似的值，这个值影响到后续噪声的缩放值，越靠后影响越大
     */
    public static float GetPerlinValue(int seed, float originXPos, float originYPos, float scale, int octaves, float persistence, float lacunarity)
    {
        persistence = Mathf.Clamp(persistence, 0, 1);

        float perlinValue = 0;
        float totalAmplitude = 0;
        for (int i = 0; i < octaves; i++)
        {
            int currentSeed = RandomNumber.GetInt(seed, i);

            float amplitude = Mathf.Pow(persistence, i);            //当前循环的振幅，或者说当前循环代表的噪声的振幅
            totalAmplitude += amplitude;                            //保留下这个振幅，最后要用来把噪声限制到 0-1

            float currentFrequency = Mathf.Pow(lacunarity, i);      //当前噪声的频率，应该就是一个缩放值吧

            float currentPerlinValue = GetSinglePerlinValue(currentSeed, originXPos, originYPos, scale / currentFrequency) * amplitude;     //把频率和缩放一起丢进去看看结果
            perlinValue += currentPerlinValue;
        }

        perlinValue /= totalAmplitude;

        return perlinValue;
    }

    /*
    public static float GetSubPerlinValue(int seed, float originPos, float scale, float persistence, float lacunarity, int index)
    {
        persistence = Mathf.Clamp(persistence, 0, 1);

        seed = RandomNumber.GetInt(seed, index);

        float amplitude = Mathf.Pow(persistence, index);    //当前循环的振幅，或者说当前循环代表的噪声的振幅

        float currentFrequency = Mathf.Pow(lacunarity, index);  //当前噪声的频率，应该就是一个缩放值吧

        float perlinValue = GetSinglePerlinValue(seed, originPos, scale * currentFrequency) * amplitude;     //把频率和缩放一起丢进去看看结果

        return perlinValue;
    }
    */
    

    public static float GetSinglePerlinValue(int seed, float originXPos, float originYPos, float scale)
    {
        float xPos = originXPos / scale;
        float yPos = originYPos / scale;

        return DoGetSinglePerlinValue(seed, xPos, yPos);
    }


    static float DoGetSinglePerlinValue(int seed, float xPos, float yPos)
    {
        int leftPos = Mathf.FloorToInt(xPos);                   //Mathf.FlooarToInt(float f)：向下找最近的int，如果float就是个int值则返回这个int
        int rightPos = leftPos + 1;
        int lowPos = Mathf.FloorToInt(yPos);
        int upPos = lowPos + 1;

        float upperRightValue = GetSmoothedFloat(seed, rightPos, upPos);
        float lowerRightValue = GetSmoothedFloat(seed, rightPos, lowPos);
        float lowerLeftValue = GetSmoothedFloat(seed, leftPos, lowPos);
        float upperLeftValue = GetSmoothedFloat(seed, leftPos, upPos);

        float xRate = xPos - leftPos;
        xRate = 1 - (Mathf.Cos(xRate * Mathf.PI) / 2 + 0.5f);

        float lowerValue = Mathf.Lerp(lowerLeftValue, lowerRightValue, xRate);
        float upperValue = Mathf.Lerp(upperLeftValue, upperRightValue, xRate);

        float yRate = yPos - lowPos;
        yRate = 1 - (Mathf.Cos(yRate * Mathf.PI) / 2 + 0.5f);

        return Mathf.Lerp(lowerValue, upperValue, yRate);
    }

    static float GetSmoothedFloat(int seed, int xPos, int yPos)
    {
        float corners = (RandomNumber.GetFloat(seed, xPos + 1, yPos + 1) + RandomNumber.GetFloat(seed, xPos + 1, yPos - 1) + RandomNumber.GetFloat(seed, xPos - 1, yPos - 1) + RandomNumber.GetFloat(seed, xPos - 1, yPos + 1)) / 16;
        float sides = (RandomNumber.GetFloat(seed, xPos, yPos + 1) + RandomNumber.GetFloat(seed, xPos + 1, yPos) + RandomNumber.GetFloat(seed, xPos, yPos - 1) + RandomNumber.GetFloat(seed, xPos - 1, yPos)) / 8;
        float center = RandomNumber.GetFloat(seed, xPos, yPos) / 4;

        return corners + sides + center;
    }
}
