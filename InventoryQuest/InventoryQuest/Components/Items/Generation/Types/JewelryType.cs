using System;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class JeweleryType : ItemType
    {
        public JeweleryType()
        {
            ResetAllSkippedStats();
        }
    }
}