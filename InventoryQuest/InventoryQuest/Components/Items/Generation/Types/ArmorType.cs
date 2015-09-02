using System;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class ArmorType : ItemType
    {
        private MinMaxStat _Armor = new MinMaxStat();

        public ArmorType()
        {
            ResetAllSkippedStats();
        }

        /// <summary>
        ///     Min, Max defend on item (not scaled with level)
        /// </summary>
        public MinMaxStat Armor
        {
            get { return _Armor; }
            set { _Armor = value; }
        }
    }
}