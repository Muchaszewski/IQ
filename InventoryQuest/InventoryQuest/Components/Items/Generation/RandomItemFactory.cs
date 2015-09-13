using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Items.Generation.Types;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Utils;

namespace InventoryQuest.Components.Generation.Items
{
    /// <summary>
    ///     Factory for items
    /// </summary>
    public static class RandomItemFactory
    {
        public const float PoorModifier = 0.25f;

        public static Item CreateItem(Spot spot, EnumItemRarity rarity = EnumItemRarity.Normal)
        {
            return CreateItem(
                RandomNumberGenerator.NextRandom(spot.Level - 3, spot.Level + 2),
                spot,
                rarity
                );
        }

        /// <summary>
        ///     Creating random item
        /// </summary>
        /// <param name="level">Level of created weapon</param>
        /// <returns></returns>
        public static Item CreateItem(int level, Spot spot, EnumItemRarity rarity = EnumItemRarity.Poor)
        {
            if (level <= 0)
            {
                level = 1;
            }
            var totalWeight = spot.ItemsTotalWeight;
            var randWeight = RandomNumberGenerator.NextRandom(0, totalWeight);
            rarity = RandomRare(rarity);
            foreach (GenerationWeightLists item in spot.ItemsList)
            {
                randWeight -= item.Weight;
                if (randWeight <= 0)
                {
                    return ChooseItemType(level, item, rarity);
                }
            }
            return null;
        }

        /// <summary>
        /// WARNING VERY ADAVANCE TOOL FOR CREATING ITEM(S) REQUIRE A LOT OF TIME TO CREATE
        /// </summary>
        /// <param name="level">Desire level that item shoudl spawn with</param>
        /// <param name="matchRarity">Rarity that item is signed with or poor to select all</param>
        /// <param name="rarity">Rarity that item shoudl spawn with</param>
        /// <param name="type">Type of item for precisiton, can NOT be NULL</param>
        /// <param name="matchStrings">String array to match with names, extra names, flavorTexts, WARNING String.Empty or Null will create every item in the game of given type</param>
        /// <returns></returns>
        public static Item[] CreateCustomItems(int level, EnumItemRarity matchRarity, EnumItemRarity rarity, EnumItemType type, params string[] matchStrings)
        {
            var list = new List<Item>();
            int enumItemType = (int)type;
            if (enumItemType >= 0 && enumItemType <= 8) //Armors
            {
                foreach (var armorType in GenerationStorage.Instance.Armors)
                {
                    foreach (var matchString in matchStrings)
                    {
                        if ((String.Equals(armorType.Name, matchString, StringComparison.CurrentCultureIgnoreCase) ||
                            String.Equals(armorType.ExtraName, matchString, StringComparison.CurrentCultureIgnoreCase) ||
                            String.Equals(armorType.FlavorText, matchString, StringComparison.CurrentCultureIgnoreCase)
                            ) &&
                            (armorType.Rarity == matchRarity || matchRarity == EnumItemRarity.Poor))
                        {
                            var itemList = new ItemsLists();
                            itemList.ArmorTypeID.Add(new GenerationWeight() { ID = armorType.ID, Weight = 100 });
                            list.Add(CreateArmorItem(level,
                                itemList,
                                rarity,
                                type));

                        }
                    }
                }
            }
            else if (enumItemType == 9 || enumItemType == 10) //Amu and Rings
            {
                //TODO
                //foreach (var jeweleryType in GenerationStorage.Instance.Jewelery)
                //{
                //    foreach (var matchString in matchStrings)
                //    {
                //        if ((jeweleryType.Name.ToLower() == matchString.ToLower() ||
                //            jeweleryType.ExtraName.ToLower() == matchString.ToLower() ||
                //            jeweleryType.FlavorText.ToLower() == matchString.ToLower()
                //            ) &&
                //            (jeweleryType.Rarity == matchRarity || matchRarity == EnumItemRarity.Poor))
                //        {
                //            var itemList = new ItemsLists();
                //            itemList.JewelerTypeID.Add(new GenerationWeight() { ID = jeweleryType.ID, Weight = 100 });
                //            list.Add(CreateOffHandItem(level,
                //                itemList,
                //                rarity,
                //                type));

                //        }
                //    }
                //}
            }
            else if (enumItemType == 11) //Shields
            {
                foreach (var shieldType in GenerationStorage.Instance.Shields)
                {
                    foreach (var matchString in matchStrings)
                    {
                        if ((String.Equals(shieldType.Name, matchString, StringComparison.CurrentCultureIgnoreCase) ||
                            String.Equals(shieldType.ExtraName, matchString, StringComparison.CurrentCultureIgnoreCase) ||
                            String.Equals(shieldType.FlavorText, matchString, StringComparison.CurrentCultureIgnoreCase)
                            ) &&
                            (shieldType.Rarity == matchRarity || matchRarity == EnumItemRarity.Poor))
                        {
                            var itemList = new ItemsLists();
                            itemList.ShieldTypeID.Add(new GenerationWeight() { ID = shieldType.ID, Weight = 100 });
                            list.Add(CreateShieldItem(level,
                                itemList,
                                rarity,
                                type));

                        }
                    }
                }
            }
            else if (enumItemType == 12) //Offhand
            {
                foreach (var offHandType in GenerationStorage.Instance.OffHands)
                {
                    foreach (var matchString in matchStrings)
                    {
                        if ((offHandType.Name.ToLower() == matchString.ToLower() ||
                            offHandType.ExtraName.ToLower() == matchString.ToLower() ||
                            offHandType.FlavorText.ToLower() == matchString.ToLower()
                            ) &&
                            (offHandType.Rarity == matchRarity || matchRarity == EnumItemRarity.Poor))
                        {
                            var itemList = new ItemsLists();
                            itemList.OffHandTypeID.Add(new GenerationWeight() { ID = offHandType.ID, Weight = 100 });
                            list.Add(CreateOffHandItem(level,
                                itemList,
                                rarity,
                                type));

                        }
                    }
                }
            }
            //13 is Unarmed
            else if (enumItemType >= 14 && enumItemType <= 24) //Weapons
            {
                foreach (var weaponType in GenerationStorage.Instance.Weapons)
                {
                    foreach (var matchString in matchStrings)
                    {
                        if ((weaponType.Name.ToLower() == matchString.ToLower() ||
                            weaponType.ExtraName.ToLower() == matchString.ToLower() ||
                            weaponType.FlavorText.ToLower() == matchString.ToLower()
                            ) &&
                            (weaponType.Rarity == matchRarity || matchRarity == EnumItemRarity.Poor))
                        {
                            var itemList = new ItemsLists();
                            itemList.WeaponTypeID.Add(new GenerationWeight() { ID = weaponType.ID, Weight = 100 });
                            list.Add(CreateWeaponItem(level,
                                itemList,
                                rarity,
                                type));

                        }
                    }
                }
            }
            else if (enumItemType >= 25 && enumItemType <= 26) //Lore
            {
                //TODO
                //foreach (var loreType in GenerationStorage.Instance.Lore)
                //{
                //    foreach (var matchString in matchStrings)
                //    {
                //        if ((loreType.Name.ToLower() == matchString.ToLower() ||
                //            loreType.ExtraName.ToLower() == matchString.ToLower() ||
                //            loreType.FlavorText.ToLower() == matchString.ToLower()
                //            ) &&
                //            (loreType.Rarity == matchRarity || matchRarity == EnumItemRarity.Poor))
                //        {
                //            var itemList = new ItemsLists();
                //            itemList.LoreTypeID.Add(new GenerationWeight() { ID = loreType.ID, Weight = 100 });
                //            CreateArmorItem(level,
                //                itemList,
                //                rarity,
                //                type);
                //        }
                //    }
                //}
            }

            return null;
        }

        private static Item ChooseItemType(int level, GenerationWeightLists listID,
            EnumItemRarity rarity = EnumItemRarity.Poor)
        {
            ItemsLists list = GenerationStorage.Instance.ItemsLists.Find(
                x => x.Name == listID.Name);
            var weight = list.Weight.Sum();
            var randWeight = RandomNumberGenerator.NextRandom(0, weight);
            for (var i = 0; i < list.Weight.Count; i++)
            {
                randWeight -= list.Weight[i];
                if (randWeight <= 0)
                {
                    return ResolveItemType(level, list, i, rarity);
                }
            }
            return null;
        }

        private static Item ResolveItemType(int level, ItemsLists itemList, int typeID,
            EnumItemRarity rarity = EnumItemRarity.Poor)
        {
            if (typeID >= 0 && typeID <= 8) //Armors
            {
                return CreateArmorItem(level, itemList, rarity, (EnumItemType)typeID);
            }
            else if (typeID == 9 || typeID == 10) //Amu and Rings
            {
                return null;
            }
            else if (typeID == 11) //Shields
            {
                return CreateShieldItem(level, itemList, rarity, (EnumItemType)typeID);
            }
            else if (typeID == 12) //Offhand
            {
                return CreateOffHandItem(level, itemList, rarity, (EnumItemType)typeID);
            }
            //13 is Unarmed
            else if (typeID >= 14 && typeID <= 27) //Weapons
            {
                return CreateWeaponItem(level, itemList, rarity, (EnumItemType)typeID);
            }
            return null;
        }

        /// <summary>
        ///     Creating new random weapon
        /// </summary>
        public static Item CreateWeaponItem(int level, ItemsLists itemsLists,
            EnumItemRarity rarity = EnumItemRarity.Poor,
            EnumItemType type = EnumItemType.Unarmed)
        {
            //Creating new item
            var item = new Item();

            //Creating item stats
            Stats stats = item.Stats;
            item.ValidSlot = EnumItemSlot.Weapon;

            var weaponType = (WeaponType)ChooseRarity(
                level,
                rarity,
                type,
                itemsLists.WeaponTypeID,
                GenerationStorage.Instance.Weapons.Cast<ItemType>().ToList());

            CreateBasics(ref item, weaponType, level, rarity);

            //Setting weapon stats
            stats.Accuracy.Base = (int)weaponType.Accuracy.GetRandomForLevel(level);
            stats.ArmorPenetration.Base = (int)weaponType.ArmorPenetration.GetRandomForLevel(level);
            stats.MinDamage.Base = (int)weaponType.MinDamage.GetRandomForLevel(level);
            stats.MaxDamage.Base = (int)weaponType.MaxDamage.GetRandomForLevel(level);
            if (stats.MinDamage.Base > stats.MaxDamage.Base)
            {
                stats.MaxDamage.Base = stats.MinDamage.Base;
            }
            stats.AttackSpeed.Base = (float)weaponType.AttackSpeed.GetRandomForLevel(level);
            stats.Deflection.Base = (int)weaponType.Deflection.GetRandomForLevel(level);

            //Item skill
            item.Skill = weaponType.Skill;
            item.Range = weaponType.Range;

            //Setting how many hands is needed to carry this weapon
            item.RequiredHands = weaponType.RequiredHands;

            //Setting magic attributes
            RandomMagicAttributes(item, level);


            //Setting item requirements
            foreach (MinMaxStatType stat in weaponType.RequiredStats)
            {
                item.RequiredStats.Add(new StatValueInt(stat.StatType) { Base = (int)stat.GetRandomForLevel(level) });
            }
            return item;
        }

        /// <summary>
        ///     Creating new random shield
        /// </summary>
        public static Item CreateShieldItem(int level, ItemsLists itemsLists,
            EnumItemRarity rarity = EnumItemRarity.Poor,
            EnumItemType type = EnumItemType.Unarmed)
        {
            //Creating new item
            var item = new Item();
            //Creating item stats
            Stats stats = item.Stats;
            item.ValidSlot = EnumItemSlot.OffHand;

            var shieldType = (ShieldType)ChooseRarity(
                level,
                rarity,
                type,
                itemsLists.ShieldTypeID,
                GenerationStorage.Instance.Shields.Cast<ItemType>().ToList());

            CreateBasics(ref item, shieldType, level, rarity);

            stats.BlockAmount.Base = (int)shieldType.BlockAmount.GetRandomForLevel(level);
            stats.BlockChance.Base = (int)shieldType.BlockChance.GetRandomForLevel(level);


            //Item skill
            item.Skill = shieldType.Skill;

            //Setting magic attributes
            RandomMagicAttributes(item, level);


            //Setting item requirements
            foreach (MinMaxStatType stat in shieldType.RequiredStats)
            {
                item.RequiredStats.Add(new StatValueInt(stat.StatType) { Base = (int)stat.GetRandom() });
            }
            return item;
        }

        /// <summary>
        ///     Creating new random shield
        /// </summary>
        public static Item CreateOffHandItem(int level, ItemsLists itemsLists,
            EnumItemRarity rarity = EnumItemRarity.Poor,
            EnumItemType type = EnumItemType.Unarmed)
        {
            //Creating new item
            var item = new Item();
            //Creating item stats
            Stats stats = item.Stats;
            item.ValidSlot = EnumItemSlot.OffHand;

            var offHandType = (OffHandType)ChooseRarity(
                level,
                rarity,
                type,
                itemsLists.OffHandTypeID,
                GenerationStorage.Instance.OffHands.Cast<ItemType>().ToList());

            CreateBasics(ref item, offHandType, level, rarity);

            //Item skill
            item.Skill = offHandType.Skill;

            //Setting magic attributes
            RandomMagicAttributes(item, level);


            //Setting item requirements
            foreach (MinMaxStatType stat in offHandType.RequiredStats)
            {
                item.RequiredStats.Add(new StatValueInt(stat.StatType) { Base = (int)stat.GetRandom() });
            }
            return item;
        }

        /// <summary>
        ///     Creating new random armor
        /// </summary>
        public static Item CreateArmorItem(int level, ItemsLists itemsLists, EnumItemRarity rarity = EnumItemRarity.Poor,
            EnumItemType type = EnumItemType.Unarmed)
        {
            //Creating new item
            var item = new Item();
            //Creating item stats
            Stats stats = item.Stats;

            var armorType = (ArmorType)ChooseRarity(
                level,
                rarity,
                type,
                itemsLists.ArmorTypeID,
                GenerationStorage.Instance.Armors.Cast<ItemType>().ToList());

            item.ValidSlot = (EnumItemSlot)(armorType.Type + 1);
            CreateBasics(ref item, armorType, level, rarity);

            stats.Armor.Base = (int)armorType.Armor.GetRandomForLevel(level);


            //Item skill
            item.Skill = armorType.Skill;

            //Setting magic attributes
            RandomMagicAttributes(item, level);


            //Setting item requirements
            foreach (MinMaxStatType stat in armorType.RequiredStats)
            {
                item.RequiredStats.Add(new StatValueInt(stat.StatType) { Base = (int)stat.GetRandom() });
            }
            return item;
        }

        private static void CreateBasics(ref Item item, ItemType type, int level, EnumItemRarity rarity)
        {
            //Set name
            item.Name = type.Name;
            item.ExtraName = type.ExtraName;
            item.FlavorText = type.FlavorText;
            item.Price.Base = level * 20;
            item.RequiredLevel = level;
            item.ItemLevel = level;

            //Set item type
            item.Type = type.Type;

            //Setting rarity of item
            item.Rarity = rarity;

            //Set image
            if (type.ImageID.Count == 0)
            {
                throw new Exception("Item must contain image");
            }
            PairTypeItem typeImage = type.ImageID[RandomNumberGenerator.NextRandom(type.ImageID.Count)];
            try
            {
                item.ImageID = ImagesNames.ResolveImage(typeImage.Type, typeImage.Item);
                //Nie można odnaleźć określonej ścieżki
            }
            catch
            {
                item.ImageID = null;
            }

            //Setting durability
            item.Durability.ChangeValues(RandomDurability(type, level));
        }

        private static ItemType ChooseRarity(int level, EnumItemRarity rarity, EnumItemType type,
            List<GenerationWeight> typeID, List<ItemType> items)
        {
            var listType = new List<ItemType>();
            switch (rarity)
            {
                case EnumItemRarity.Poor:
                    foreach (GenerationWeight value in typeID)
                    {
                        ItemType tempItem = items[value.ID];
                        if (tempItem.Rarity == EnumItemRarity.Poor &&
                            tempItem.DropLevel <= level)
                        {
                            listType.Add(tempItem);
                        }
                    }
                    break;
                case EnumItemRarity.Normal:
                    foreach (GenerationWeight value in typeID)
                    {
                        ItemType tempItem = items[value.ID];
                        if ((tempItem.Rarity == EnumItemRarity.Poor ||
                             tempItem.Rarity == EnumItemRarity.Normal) &&
                            tempItem.DropLevel <= level)
                        {
                            listType.Add(tempItem);
                        }
                    }
                    break;
                case EnumItemRarity.Uncommon:
                    foreach (GenerationWeight value in typeID)
                    {
                        ItemType tempItem = items[value.ID];
                        if ((tempItem.Rarity == EnumItemRarity.Poor ||
                             tempItem.Rarity == EnumItemRarity.Normal ||
                             tempItem.Rarity == EnumItemRarity.Uncommon) &&
                            tempItem.DropLevel <= level)
                        {
                            listType.Add(tempItem);
                        }
                    }
                    break;
                case EnumItemRarity.Rare:
                    foreach (GenerationWeight value in typeID)
                    {
                        ItemType tempItem = items[value.ID];
                        if ((tempItem.Rarity == EnumItemRarity.Poor ||
                             tempItem.Rarity == EnumItemRarity.Normal ||
                             tempItem.Rarity == EnumItemRarity.Uncommon ||
                             tempItem.Rarity == EnumItemRarity.Rare) &&
                            tempItem.DropLevel <= level)
                        {
                            listType.Add(tempItem);
                        }
                    }
                    break;
                case EnumItemRarity.Mythical:
                    foreach (GenerationWeight value in typeID)
                    {
                        ItemType tempItem = items[value.ID];
                        if (tempItem.Rarity == EnumItemRarity.Mythical &&
                            tempItem.DropLevel <= level)
                        {
                            listType.Add(tempItem);
                        }
                    }
                    break;
            }
            if (type == EnumItemType.Unarmed)
            {
                if (listType.Count > 0)
                {
                    var random = RandomNumberGenerator.NextRandom(listType.Count);
                    return listType[random];
                }
            }
            else
            {
                List<ItemType> list = listType.FindAll(x => x.Type == type);
                if (list.Any())
                {
                    var random = RandomNumberGenerator.NextRandom(list.Count());
                    return list[random];
                }
                if (listType.Count > 0)
                {
                    var random = RandomNumberGenerator.NextRandom(listType.Count);
                    return listType[random];
                }
            }
            var r = RandomNumberGenerator.NextRandom(typeID.Count);
            return items[typeID[r].ID];
        }

        /// <summary>
        ///     Retrun base item durability
        /// </summary>
        /// <param name="damaged">Set minimal current Durability</param>
        private static StatValueFloat RandomDurability(ItemType item, int level, int damaged = 0)
        {
            var durability = new StatValueFloat(EnumTypeStat.Durability);
            durability.Base = (int)RandomNumberGenerator.NextRandom(item.Durability.Min, item.Durability.Max);
            if (damaged != 0)
            {
                durability.Shred(damaged);
            }
            return durability;
        }

        /// <summary>
        ///     Return how epic item is
        /// </summary>
        private static EnumItemRarity RandomRare(EnumItemRarity rarity)
        {
            var attrib = new List<RarityChance>();
            var maxRand = 0;
            MemberInfo[] mem = typeof(EnumItemRarity).GetMembers();
            var index = 0;
            foreach (MemberInfo item in mem)
            {
                if (item.DeclaringType == typeof(EnumItemRarity))
                {
                    if (item.GetCustomAttributes(true).Length != 0)
                    {
                        if (index < (int)rarity)
                        {
                            index++;
                            var attribute = (RarityChance)item.GetCustomAttributes(true)[0];
                            attrib.Add(attribute);
                            maxRand += attribute.Chance;
                        }
                    }
                }
            }

            var rand = RandomNumberGenerator.NextRandom(maxRand);

            for (var i = 0; i < attrib.Count; i++)
            {
                rand -= attrib[i].Chance;
                if (rand <= 0)
                {
                    return (EnumItemRarity)i;
                }
            }
            return EnumItemRarity.Poor;
        }

        private static void RandomMagicAttributes(Item item, int level)
        {
            switch (item.Rarity)
            {
                case EnumItemRarity.Poor:
                    foreach (StatValueFloat statValueFloat in item.Stats.GetAllStatsFloat())
                    {
                        if (statValueFloat.Base > 0)
                        {
                            //TODO
                            //statValueFloat.Base = Math.Max(statValueFloat.Base*(1 - PoorModifier), 0.01f);
                        }
                    }
                    foreach (StatValueInt statValueInt in item.Stats.GetAllStatsInt())
                    {
                        if (statValueInt.Base > 0)
                        {
                            statValueInt.Base = (int)(Math.Max(statValueInt.Base * (1 - PoorModifier), 1));
                        }
                    }
                    break;

                case EnumItemRarity.Rare:
                    if (RandomNumberGenerator.BoolRandom(25))
                    {
                        RandomMagicAttribute(item, level, 3);
                    }
                    if (RandomNumberGenerator.BoolRandom(25))
                    {
                        RandomMagicAttribute(item, level, 4);
                    }
                    if (RandomNumberGenerator.BoolRandom(25))
                    {
                        RandomMagicAttribute(item, level, 5);
                    }
                    if (RandomNumberGenerator.BoolRandom(25))
                    {
                        RandomMagicAttribute(item, level, 6);
                    }
                    goto case EnumItemRarity.Uncommon;

                case EnumItemRarity.Uncommon:
                    if (RandomNumberGenerator.BoolRandom(50))
                    {
                        RandomMagicAttribute(item, level, 1);
                    }
                    else
                    {
                        RandomMagicAttribute(item, level, 1);
                        RandomMagicAttribute(item, level, 2);
                    }
                    break;

                case EnumItemRarity.Mythical:
                    // TEMP, TODO
                    if (RandomNumberGenerator.BoolRandom(50))
                    {
                        RandomMagicAttribute(item, level + 20, 1);
                    }
                    else
                    {
                        RandomMagicAttribute(item, level + 10, 1);
                        RandomMagicAttribute(item, level + 10, 2);
                    }
                    break;
            }
        }

        private static void RandomMagicAttribute(Item item, int level, int statNumber)
        {
            var statsI = item.Stats.GetAllStatsInt().Count();
            var statsF = item.Stats.GetAllStatsFloat().Count();

            int bonusStat;
            switch (statNumber)
            {
                case 1:
                    bonusStat = RandomNumberGenerator.NextRandom(level, level + 5);
                    break;
                case 2:
                    bonusStat = RandomNumberGenerator.NextRandom(level - 3, level);
                    break;
                case 3:
                    bonusStat = RandomNumberGenerator.NextRandom(level, level + 2);
                    break;
                case 4:
                    bonusStat = RandomNumberGenerator.NextRandom(level - 1, level + 1);
                    break;
                case 5:
                    bonusStat = RandomNumberGenerator.NextRandom(level - 2, level);
                    break;
                case 6:
                    bonusStat = RandomNumberGenerator.NextRandom(level - 3, level - 1);
                    break;
                default:
                    throw new Exception("Bad number of magic stats");
            }

            if (bonusStat <= 0)
            {
                bonusStat = 1;
            }

            try
            {
                var statR = RandomNumberGenerator.NextRandom(statsI + statsF);
                if (statR < statsI)
                {
                    StatValueInt stat = item.Stats.GetAllStatsInt()[statR];
                    StatScaleAttribute scale = StatScaleAttribute.GetAttributeByName(Enum.GetName(typeof(EnumTypeStat), stat.Type));
                    if (stat.Base == 0)
                    {
                        stat.Base = bonusStat * Convert.ToInt32(scale.Scale);
                    }
                    else
                    {
                        stat.Extend += bonusStat * Convert.ToInt32(scale.Scale);
                    }
                }
                else
                {
                    statR -= statsI;
                    StatValueFloat stat = item.Stats.GetAllStatsFloat()[statR];
                    StatScaleAttribute scale =
                        StatScaleAttribute.GetAttributeByName(Enum.GetName(typeof(EnumTypeStat), stat.Type));
                    if (stat.Base == 0)
                    {
                        stat.Base = bonusStat * scale.Scale;
                    }
                    else
                    {
                        stat.Extend += bonusStat * scale.Scale;
                    }
                }
            }
            catch
            { }
        }
    }
}