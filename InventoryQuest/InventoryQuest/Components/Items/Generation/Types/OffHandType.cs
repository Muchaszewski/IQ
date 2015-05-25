using System;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class OffHandType : ItemType
    {
        public OffHandType()
        {
            ResetAllSkippedStats();
        }
    }
}