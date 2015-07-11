using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Xml.Serialization;
using InventoryQuest.Components.Entities.Generation.Types;
using InventoryQuest.Components.Items.Generation.Types;
using UnityEngine;

namespace InventoryQuest.Components
{
    [Serializable]
    public class GenerationStorage
    {
        private static GenerationStorage _Instance;
#if CREATOR
        public static readonly string XMLPath = "Storage.xml";
#else
        public static readonly string XMLPath = "Storage";
#endif
        private List<ArmorType> _Armors = new List<ArmorType>();
        private List<EntityType> _Entities = new List<EntityType>();
        private List<EntityLists> _EntityLists = new List<EntityLists>();
        private List<EntityType> _EntityTemplateList = new List<EntityType>();
        private List<ItemsLists> _ItemsLists = new List<ItemsLists>();
        private List<Jewelery> _Jewelery = new List<Jewelery>();
        private List<OffHandType> _OffHands = new List<OffHandType>();
        private List<ShieldType> _Shields = new List<ShieldType>();
        private List<Spot> _Spots = new List<Spot>();
        private List<WeaponType> _Weapons = new List<WeaponType>();

        static GenerationStorage()
        {
            if (_Instance == null)
            {
                _Instance = LoadXml("");
            }
        }

        public static GenerationStorage Instance
        {
            get { return _Instance; }
        }

        public List<EntityType> Entities
        {
            get { return _Entities; }
            set { _Entities = value; }
        }

        public List<WeaponType> Weapons
        {
            get { return _Weapons; }
            set { _Weapons = value; }
        }

        public List<ArmorType> Armors
        {
            get { return _Armors; }
            set { _Armors = value; }
        }

        public List<ShieldType> Shields
        {
            get { return _Shields; }
            set { _Shields = value; }
        }

        public List<OffHandType> OffHands
        {
            get { return _OffHands; }
            set { _OffHands = value; }
        }

        public List<Jewelery> Jewelery
        {
            get { return _Jewelery; }
            set { _Jewelery = value; }
        }

        public List<Spot> Spots
        {
            get { return _Spots; }
            set { _Spots = value; }
        }

        public List<ItemsLists> ItemsLists
        {
            get { return _ItemsLists; }
            set { _ItemsLists = value; }
        }

        public List<EntityLists> EntityLists
        {
            get { return _EntityLists; }
            set { _EntityLists = value; }
        }

        public List<EntityType> EntityTemplateList
        {
            get { return _EntityTemplateList; }
            set { _EntityTemplateList = value; }
        }

        public static void SetInstance(GenerationStorage gs)
        {
            _Instance = gs;
        }

        public static GenerationStorage LoadXml(string path = "", bool isDefault = true)
        {
            var serializer = new XmlSerializer(typeof(GenerationStorage));
            if (isDefault)
            {
                path += XMLPath;
            }
#if CREATOR
            if (File.Exists(path))
            {
                var gs = new GenerationStorage();
                TextReader file = new StreamReader(path);
                gs = (GenerationStorage)serializer.Deserialize(file);
                file.Close();
                return gs;
            }
#else
                var file = Resources.Load(path) as TextAsset;
                if (file != null)
                {
                    var gs = new GenerationStorage();
                    gs = (GenerationStorage)serializer.Deserialize(new StringReader(file.text));
                    return gs;
                }
                else
                {
                    throw new Exception(Directory.GetFiles(path).Aggregate((x, y) => x.ToString()));
                }
#endif
            return new GenerationStorage();
        }

        public static void SaveXml(string path)
        {
            var serializer = new XmlSerializer(typeof(GenerationStorage));
            MemoryStream memory;
            using (memory = new MemoryStream())
            {
                serializer.Serialize(memory, Instance);
                if (path == "" || path == string.Empty)
                {
                    path = XMLPath;
                }
                else
                {
                    path += "\\" + XMLPath;
                    using (Stream file = File.Create(path))
                    {
                        memory.Position = 0;
                        memory.WriteTo(file);
                    }
                }
                using (Stream file = File.Create(XMLPath))
                {
                    memory.Position = 0;
                    memory.WriteTo(file);
                }
            }
        }
    }
}