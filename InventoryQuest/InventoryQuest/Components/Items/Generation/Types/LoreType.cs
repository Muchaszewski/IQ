using InventoryQuest.Components.Items.Generation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class LoreType : ItemType
    {
        public LoreType()
        {
            ResetAllSkippedStats();
        }
    }
}
