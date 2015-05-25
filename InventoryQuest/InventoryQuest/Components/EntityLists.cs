using System.Collections.Generic;

namespace InventoryQuest.Components
{
    public class EntityLists
    {
        private List<GenerationWeight> _EntityTypeNormalID = new List<GenerationWeight>();
        private List<GenerationWeight> _EntityTypeRareID = new List<GenerationWeight>();
        private List<GenerationWeight> _EntityTypeUncommonID = new List<GenerationWeight>();
        private List<GenerationWeight> _EntityTypeUniqueID = new List<GenerationWeight>();
        public string Name { get; set; }

        public int Count
        {
            get
            {
                return EntityTypeNormalID.Count + EntityTypeRareID.Count + EntityTypeUncommonID.Count +
                       EntityTypeUniqueID.Count;
            }
        }

        public List<GenerationWeight> EntityTypeNormalID
        {
            get { return _EntityTypeNormalID; }
            set { _EntityTypeNormalID = value; }
        }

        public List<GenerationWeight> EntityTypeUncommonID
        {
            get { return _EntityTypeUncommonID; }
            set { _EntityTypeUncommonID = value; }
        }

        public List<GenerationWeight> EntityTypeRareID
        {
            get { return _EntityTypeRareID; }
            set { _EntityTypeRareID = value; }
        }

        public List<GenerationWeight> EntityTypeUniqueID
        {
            get { return _EntityTypeUniqueID; }
            set { _EntityTypeUniqueID = value; }
        }
    }
}