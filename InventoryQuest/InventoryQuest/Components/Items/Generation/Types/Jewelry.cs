using System;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class Jewelery : ItemType
    {
        public Jewelery()
        {
            ResetAllSkippedStats();
        }
    }
}