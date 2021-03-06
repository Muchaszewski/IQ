﻿using InventoryQuest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventoryQuest.Components
{
    [Serializable]
    public class Spot
    {
        [field: NonSerialized]
        public static event EventHandler onNewAreaUnlocked = delegate { };

        private List<GenerationWeightLists> _entitiesList = new List<GenerationWeightLists>();
        private List<GenerationWeightLists> _itemsList = new List<GenerationWeightLists>();
        private List<GenerationWeightLists> _itemOnComplete = new List<GenerationWeightLists>();
        private List<SpotConnection> _listConnections = new List<SpotConnection>();
        private int _progress;

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
        public int MonsterValueToCompleteArea { get; set; }
        public bool IsUnlocked { get; set; }

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

        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress >= MonsterValueToCompleteArea)
                {
                    UnlockConnections();
                }
                _progress = value;
            }
        }

        public List<SpotConnection> ListConnections
        {
            get { return _listConnections; }
            set { _listConnections = value; }
        }

        [Serializable]
        public class SpotConnection
        {
            public string SpotString { get; set; }
            public int Distance { get; set; }
            public bool IsTwoWay { get; set; }
        }

        public void UnlockConnections()
        {
            foreach (var connection in ListConnections)
            {
                var spot = FindSpotByConnection(connection);
                if (spot != null)
                {
                    spot.IsUnlocked = true;
                }
                else
                {
                    Debug.LogError("Connection list was not empty but connection name was not found");
                    return;
                }
            }
            onNewAreaUnlocked.Invoke(this, EventArgs.Empty);
        }

        public static Spot FindSpotByConnection(Spot.SpotConnection connection)
        {
            return GenerationStorage.Instance.Spots.FirstOrDefault(x => x.Name.Equals(connection.SpotString));
        }

        public static Spot.SpotConnection FindConnectionBySpot(Spot spot, Spot connected)
        {
            return spot.ListConnections.FirstOrDefault(x => x.SpotString.Equals(connected.Name));
        }
    }
}