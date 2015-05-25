using System;

namespace InventoryQuest
{
    /// <summary>
    ///     Basic min max for stats
    /// </summary>
    [Serializable]
    public class MinMaxStat
    {
        private float _Max;
        private float _MaxMaxLevel;
        private float _Min;
        private float _MinMaxLevel;

        public MinMaxStat()
        {
        }

        /// <summary>
        ///     Minimal/Maximal stat that item at TODO level can spawn with </para>
        ///     If you want to scale this stat use MinMaxStat(min, max, minMaxLevel, maxMaxLevel)
        /// </summary>
        /// <param name="min">TODO level stat</param>
        /// <param name="max">TODO level stat</param>
        public MinMaxStat(float min, float max)
        {
            _Min = min;
            _Max = max;
        }

        /// <summary>
        ///     Minimal/Maximal stat that item at TODO level and 100 level can spawn with
        /// </summary>
        /// <param name="min">TODO level stats</param>
        /// <param name="max">TODO level stat</param>
        /// <param name="minMaxLevel">100 level stats</param>
        /// <param name="maxMaxLevel">100 level stats</param>
        public MinMaxStat(float min, float max, float minMaxLevel, float maxMaxLevel)
            : this(min, max)
        {
            MinMaxLevel = minMaxLevel;
            MaxMaxLevel = MaxMaxLevel;
        }

        /// <summary>
        ///     Minimal stat that item at MinLevel level can spawn with
        /// </summary>
        public float Min
        {
            get { return _Min; }
            set { _Min = value; }
        }

        /// <summary>
        ///     Maximal stat that item at MinLevel level can spawn with
        /// </summary>
        public float Max
        {
            get { return _Max; }
            set { _Max = value; }
        }

        /// <summary>
        ///     Minimal stat that item at 100 level can spawn with
        /// </summary>
        public float MinMaxLevel
        {
            get { return _MinMaxLevel; }
            set { _MinMaxLevel = value; }
        }

        /// <summary>
        ///     Maximal stat that item at 100 level can spawn with
        /// </summary>
        public float MaxMaxLevel
        {
            get { return _MaxMaxLevel; }
            set { _MaxMaxLevel = value; }
        }

        public float GetRandom()
        {
            return RandomNumberGenerator.NextRandom(Min, Max);
        }

        public float GetMaxRandom()
        {
            return RandomNumberGenerator.NextRandom(MinMaxLevel, MaxMaxLevel);
        }

        public double GetStatForLevel(int level)
        {
            return 0.00005*Math.Pow(level, 2) + 0.005*level;
        }

        /// <summary>
        ///     Return Value based on Item minimal level, and Item current level
        /// </summary>
        /// <param name="minLevel">Item required level</param>
        /// <param name="currentLevel">Item drop level</param>
        /// <returns></returns>
        public float GetRandomForLevel(int level)
        {
            if (level <= 0)
            {
                level = 1;
            }
            if (MinMaxLevel == 0 && MaxMaxLevel == 0)
            {
                return GetRandom();
            }
            var min = (float) ( Min + GetStatForLevel(level)*( MinMaxLevel - Min));
            var max = (float) ( Max + GetStatForLevel(level)*( MaxMaxLevel - Max));
            return RandomNumberGenerator.NextRandom(min, max);
        }

        public override string ToString()
        {
            if ( MaxMaxLevel != 0)
            {
                return Min + "/" + Max + "; " + MinMaxLevel + "/" + MaxMaxLevel;
            }
            if ( Min ==  Max)
            {
                return Min + "";
            }
            return Min + "/" + Max;
            ;
        }
    }
}