using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace InventoryQuest.InventoryQuest.Quests
{
    [Serializable]
    public class Quest
    {
        /// <summary>
        /// Total progress of given quest in percent
        /// </summary>
        public int TotalProgress { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
    }
}
