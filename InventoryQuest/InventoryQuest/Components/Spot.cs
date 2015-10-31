using InventoryQuest.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryQuest.Components
{
    [Serializable]
    public class Spot
    {
        private List<GenerationWeightLists> _entitiesList = new List<GenerationWeightLists>();
        private List<GenerationWeightLists> _itemsList = new List<GenerationWeightLists>();
        private List<GenerationWeightLists> _itemOnComplete = new List<GenerationWeightLists>();

        public Spot()
        {
            ListConnections = new List<SpotConnection>();
        }

        public Spot(string name) : base()
        {
            Name = name;
        }

        public string Name { get; set; }
        public int Level { get; set; }
        public int ID { get; set; }
        public string ImageString { get; set; }
        public int MonsterValueToCompleteArea { get; set; }
        public List<SpotConnection> ListConnections { get; set; }

        public float Size { get; set; }
        public Vector3 Position { get; set; }

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
        public List<GenerationWeightLists> ItemOnComplete
        {
            get { return _itemOnComplete; }
            set { _itemOnComplete = value; }
        }

        public int ItemOnCompleteTotalWeight
        {
            get
            {
                var weight = 0;
                foreach (GenerationWeightLists item in ItemOnComplete)
                {
                    weight = item.Weight;
                }
                return weight;
            }
        }

        [Serializable]
        public class SpotConnection
        {
            public string SpotString { get; set; }
            public int Distance { get; set; }
            public bool IsTwoWay { get; set; }
        }
    }
}