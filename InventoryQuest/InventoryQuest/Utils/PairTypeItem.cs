using System;

namespace InventoryQuest.Utils
{
    [Serializable]
    public class PairTypeItem
    {
        private string _Item;
        private string _Type;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public string Item
        {
            get { return _Item; }
            set { _Item = value; }
        }

        public override string ToString()
        {
            return "(" + Type + ", " + Item + ")";
        }
    }
}