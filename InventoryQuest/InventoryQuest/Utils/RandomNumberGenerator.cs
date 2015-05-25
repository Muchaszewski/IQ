using System;

namespace InventoryQuest
{
    public static class RandomNumberGenerator
    {
        private static readonly Random random = new Random();

        /// <summary>
        ///     Random int
        /// </summary>
        /// <returns></returns>
        public static int NextRandom()
        {
            return random.Next();
        }

        /// <summary>
        ///     Radnom int lesser then max
        /// </summary>
        /// <param name="max">Max</param>
        /// <returns></returns>
        public static int NextRandom(int max)
        {
            return random.Next(max);
        }

        /// <summary>
        ///     Random int between min and max
        /// </summary>
        /// <param name="min">Min</param>
        /// <param name="max">
        ///     <Max/ param>
        ///         <returns></returns>
        public static int NextRandom(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        ///     Random float between min and max
        /// </summary>
        /// <param name="min">Min</param>
        /// <param name="max">Max</param>
        /// <returns></returns>
        public static float NextRandom(float min, float max)
        {
            return (float) (random.NextDouble()*(max - min) + min);
        }

        /// <summary>
        ///     Return true if given procent is bigger then random of 100
        /// </summary>
        /// <param name="procent">Number between 0 - 100</param>
        /// <returns></returns>
        public static bool BoolRandom(int procent)
        {
            var max = NextRandom(100);
            if (max > procent)
            {
                return true;
            }
            return false;
        }
    }
}