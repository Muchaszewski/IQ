using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InventoryQuest.Components.Statistics
{
    /// <summary>
    ///     Attribute to scale stat generation within one creation algorithm
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StatScaleAttribute : Attribute
    {
        private static readonly List<StatScaleAttribute> statScaleList = new List<StatScaleAttribute>();

        static StatScaleAttribute()
        {
            MemberInfo[] enumTypeStatMembers = typeof(EnumTypeStat).GetMembers();
            foreach (MemberInfo item in enumTypeStatMembers)
            {
                if (item.DeclaringType == typeof(EnumTypeStat) &&
                    item.GetCustomAttributes(true).Length != 0 &&
                    item.Name != "Unknown")
                {
                    foreach (var attribute in item.GetCustomAttributes(typeof(StatScaleAttribute), false))
                    {
                        if (attribute.GetType() == typeof(StatScaleAttribute))
                        {
                            statScaleList.Add(attribute as StatScaleAttribute);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Create new scale for stat
        /// </summary>
        /// <param name="scale"></param>
        public StatScaleAttribute(float scale)
        {
            Scale = scale;
        }

        /// <summary>
        ///     Get/Set scale of this stat
        /// </summary>
        public float Scale { get; set; }

        public static StatScaleAttribute GetAttributeByName(string name)
        {
            string[] collection = Enum.GetNames(typeof(EnumTypeStat));
            var attrib = new StatScaleAttribute(0);
            for (var i = 0; i < collection.Count(); i++)
            {
                if (collection[i] == name)
                {
                    attrib = statScaleList[i];
                    break;
                }
            }

            return attrib;
        }
    }
}