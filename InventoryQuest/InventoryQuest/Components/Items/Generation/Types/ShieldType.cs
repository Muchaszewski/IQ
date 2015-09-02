using System;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class ShieldType : ItemType
    {
        private MinMaxStat _BlockAmount = new MinMaxStat();
        private MinMaxStat _BlockChance = new MinMaxStat();

        public ShieldType()
        {
            ResetAllSkippedStats();
        }

        /// <summary>
        ///     Min, Max Block chance on item
        /// </summary>
        public MinMaxStat BlockAmount
        {
            get { return _BlockAmount; }
            set { _BlockAmount = value; }
        }

        /// <summary>
        ///     Min, Max Block chance on item
        /// </summary>
        public MinMaxStat BlockChance
        {
            get { return _BlockChance; }
            set { _BlockChance = value; }
        }
    }
}