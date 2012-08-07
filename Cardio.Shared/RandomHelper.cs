using System;

namespace Cardio.UI
{
    public static class RandomHelper
    {
        private static readonly Random _random = new Random(); 

        public static float GetRandomFloatFromInterval(float min, float max)
        {
            float diff = max - min;
            return min + (float) _random.NextDouble()*diff;
        }

        /// <summary>
        /// Returns a random integer from specified interval
        /// </summary>
        /// <param name="start">Inlcusive lower bound</param>
        /// <param name="max">Inclusive upper bound</param>
        /// <returns></returns>
        public static int RandomFrom(int start, int max)
        {
            // + 1 as max value is exclusive in default Random
            return _random.Next(start, max + 1);
        }
    }
}
