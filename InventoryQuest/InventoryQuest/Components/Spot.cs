using InventoryQuest.Utils;
using System;
using System.Collections.Generic;

namespace InventoryQuest.Components
{
    [Serializable]
    public class Spot
    {
        private List<GenerationWeightLists> _entitiesList = new List<GenerationWeightLists>();
        private List<GenerationWeightLists> _itemsList = new List<GenerationWeightLists>();

        public Spot()
        {
        }

        public Spot(string name) : base()
        {
            Name = name;
        }

        public string Name { get; set; }
        public int Level { get; set; }
        public int ID { get; set; }
        public string ImageString { get; set; }

        public string Category { get; set; }

        public List<GenerationWeightLists> EntitiesList
        {
            get { return _entitiesList; }
            set { _entitiesList = value; }
        }

        public int EntitiesTotalWeight
        {
            get
            {
                var weight = 0;
                foreach (GenerationWeightLists item in EntitiesList)
                {
                    weight = item.Weight;
                }
                return weight;
            }
        }

        public List<GenerationWeightLists> ItemsList
        {
            get { return _itemsList; }
            set { _itemsList = value; }
        }

        public int ItemsTotalWeight
        {
            get
            {
                var weight = 0;
                foreach (GenerationWeightLists item in ItemsList)
                {
                    weight = item.Weight;
                }
                return weight;
            }
        }
    }
}