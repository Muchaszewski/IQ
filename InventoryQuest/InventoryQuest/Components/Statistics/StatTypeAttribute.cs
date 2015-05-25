using System;

namespace InventoryQuest.Components.Statistics
{
    /// <summary>
    ///     Default stat for given item type
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class StatTypeAttribute : Attribute
    {
        public StatTypeAttribute(EnumStatItemPartType type)
        {
            Type = type;
        }

        public EnumStatItemPartType Type { get; private set; }
    }
}