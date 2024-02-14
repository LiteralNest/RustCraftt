using UnityEngine;

namespace CustomMathSystem
{
    public static class CustomMath
    {
        public static int GetParsedFloatToInt(float floatValue)
        {
            int intValue = Mathf.RoundToInt(floatValue);
            if (floatValue - intValue >= 0.51f)
                intValue++;
            return intValue;
        }
    }
}