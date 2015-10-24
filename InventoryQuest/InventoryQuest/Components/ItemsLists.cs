using System.Collections.Generic;

namespace InventoryQuest.Components
{
    public class ItemsLists
    {
        private EnumItemListType _enumItemListType = EnumItemListType.General;
        private List<GenerationWeight> _ArmorTypeID = new List<GenerationWeight>();
        private List<GenerationWeight> _JeweleryTypeID = new List<GenerationWeight>();
        private List<GenerationWeight> _OffHandTypeID = new List<GenerationWeight>();
        private List<GenerationWeight> _ShieldTypeID = new List<GenerationWeight>();
        private List<GenerationWeight> _WeaponTypeID = new List<GenerationWeight>();
        private List<GenerationWeight> _LoreTypeID = new List<GenerationWeight>();
        private List<int> _Weight = new List<int>();
        public string Name { get; set; }

        public int Count
        {
            get
            {
                return WeaponTypeID.Count + ArmorTypeID.Count + ShieldTypeID.Count + OffHandTypeID.Count +
                       JewelerTypeID.Count;
            }
        }

        public List<int> Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }

        public List<GenerationWeight> WeaponTypeID
        {
            get { return _WeaponTypeID; }
            set { _WeaponTypeID = value; }
        }

        public List<GenerationWeight> ArmorTypeID
        {
            get { return _ArmorTypeID; }
            set { _ArmorTypeID = value; }
        }

        public List<GenerationWeight> ShieldTypeID
        {
            get { return _ShieldTypeID; }
            set { _ShieldTypeID = value; }
        }

        public List<GenerationWeight> OffHandTypeID
        {
            get { return _OffHandTypeID; }
            set { _OffHandTypeID = value; }
        }

        public List<GenerationWeight> JewelerTypeID
        {
            get { return _JeweleryTypeID; }
            set { _JeweleryTypeID = value; }
        }

        public List<GenerationWeight> LoreTypeID
        {
            get { return _LoreTypeID; }
            set { _LoreTypeID = value; }
        }

        public EnumItemListType ItemListType
        {
            get { return _enumItemListType; }
            set { _enumItemListType = value; }
        }
    }

    public enum EnumItemListType
    {
        //For items drop form casual monsters
        General,
        Special,
        Lore,
        Bestiary,
    }
}