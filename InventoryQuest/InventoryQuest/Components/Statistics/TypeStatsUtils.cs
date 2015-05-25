using System;
using System.Collections.Generic;
using System.Reflection;

namespace InventoryQuest.Components.Statistics
{
    public static class TypeStatsUtils
    {
        private static readonly List<NameAttribute> cachedTypeStats = new List<NameAttribute>();

        static TypeStatsUtils()
        {
            CacheTypeStatsInfos();
        }

        public static NameAttribute GetNameAttribute(int value)
        {
            return cachedTypeStats[value];
        }

        private static void CacheTypeStatsInfos()
        {
            var typeStats = (EnumTypeStat[]) Enum.GetValues(typeof (EnumTypeStat));
            MemberInfo[] typeStatsMembers =
                typeof (EnumTypeStat).GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);

            for (var i = 0; i < typeStats.Length; i++)
            {
                EnumTypeStat c = typeStats[i];

                cachedTypeStats.Add(NameAttribute.GetNameAttribute(typeStatsMembers[i]));
            }
        }
    }
}