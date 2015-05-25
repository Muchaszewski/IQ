using System;
using InventoryQuest.Components.Statistics;

namespace InventoryQuest
{
    [Serializable]
    public class MinMaxStatType : MinMaxStat
    {
        private EnumTypeStat _StatType;

        public MinMaxStatType()
        {
        }

        public MinMaxStatType(EnumTypeStat statType, float min, float max)
        {
            StatType = statType;
            Min = min;
            Max = max;
        }

        public EnumTypeStat StatType
        {
            get { return _StatType; }
            set { _StatType = value; }
        }

        public override string ToString()
        {
            if (Math.Abs(MaxMaxLevel) < 0.005)
            {
                return Min + "/" + Max + "; " + MinMaxLevel + "/" + MaxMaxLevel + " | " + StatType;
            }
            return Min + "/" + Max + " | " + StatType;
        }
    }
}