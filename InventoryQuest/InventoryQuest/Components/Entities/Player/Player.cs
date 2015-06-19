using System;
using InventoryQuest.Components.Entities.Money;
using InventoryQuest.Components.Entities.Player.Inventory;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;

namespace InventoryQuest.Components.Entities.Player
{
    /// <summary>
    ///     Player class
    /// </summary>
    [Serializable]
    public sealed class Player : Entity
    {
        private Equipment _equipment;
        private double _Experience;
        private Inventory.Inventory _inventory;
        private PasiveSkills _pasiveSkills;
        private Wallet _wallet;

        /// <summary>
        ///     Player Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="level">Level</param>
        /// <param name="stats">Stats</param>
        public Player(string name, int level, Stats stats, EnumEntityType entityType = EnumEntityType.Player)
            : base(name, level, stats, entityType)
        {
            Wallet = new Wallet();
            Inventory = new Inventory.Inventory();
            Equipment = new Equipment(this);
            PasiveSkills = new PasiveSkills();
            SetAllBaseStats();
        }

        /// <summary>
        ///     This player inventory
        /// </summary>
        public Inventory.Inventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        /// <summary>
        ///     This player equiped equipment
        /// </summary>
        public Equipment Equipment
        {
            get { return _equipment; }
            set { _equipment = value; }
        }

        /// <summary>
        ///     Experience of pasive skills
        /// </summary>
        public PasiveSkills PasiveSkills
        {
            get { return _pasiveSkills; }
            set { _pasiveSkills = value; }
        }

        public Wallet Wallet
        {
            get { return _wallet; }
            set { _wallet = value; }
        }

        /// <summary>
        ///     Experience
        /// </summary>
        public double Experience
        {
            get { return _Experience; }
            set
            {
                _Experience = value;
                TryPromote();
            }
        }

        /// <summary>
        ///     Return damage based on stats and equipment
        /// </summary>
        /// <returns></returns>
        public override int Attack(out int critical)
        {
            if (Equipment.Weapon != null && Equipment.Weapon.Name != "Unarmed")
            {
                Item item = Equipment.Weapon;
                var damage = base.Attack(out critical);
                PasiveSkills.AddSkillExperienceByEnum(Equipment.Weapon.Skill, damage);
                return damage;
            }
            else
            {
                var damage = base.Attack(out critical);
                PasiveSkills.AddSkillExperienceByEnum(EnumItemClassSkill.Unarmed, damage);
                return damage;
            }
        }

        [Obsolete]
        private int calculateDamage(EnumTypeStat stat1, EnumTypeStat stat2)
        {
            return ((Stats.GetStatIntByEnum(stat1).Current + Stats.GetStatIntByEnum(stat2).Current)/2 +
                    Equipment.Weapon.Stats.MinDamage.Current*
                    (int) (1 + PasiveSkills.GetSkillLevelByEnum(Equipment.Weapon.Skill)/100f));
        }

        /// <summary>
        ///     Return defend based on stats and equipment
        /// </summary>
        /// <returns></returns>
        public override int Defend()
        {
            var defence = 0;
            foreach (Item item in Equipment.Armor)
            {
                if (item != null)
                {
                    var def = item.Stats.Armor.Current;
                    PasiveSkills.AddSkillExperienceByEnum(item.Skill, def);
                    defence += def;
                }
            }
            if (Equipment.OffHand != null)
            {
                Item item = Equipment.OffHand;
                var def = item.Stats.Armor.Current;
                PasiveSkills.AddSkillExperienceByEnum(item.Skill, def);
                return defence + def;
            }
            return defence;
        }

        /// <summary>
        ///     Return atack speed based on stats and equipment
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public float CreateAttackSpeed()
        {
            if (Equipment != null && Equipment.Weapon != null && Equipment.Weapon.Name != "Unarmed")
            {
                Stats ws = Equipment.Weapon.Stats;
                return ws.AttackSpeed.Current;
            }
            return -999f;
        }

        /// <summary>
        ///     Create player accuracy based
        /// </summary>
        /// <returns></returns>
        public void SetCustomAccuracy()
        {
            if (Equipment.Weapon == null)
            {
                Stats.Accuracy.Base = 0;
                return;
            }
            // ( ( attribute_1 + attribute_2 ) * 2 + weapon_skilli * 5 ) * weapon_accuracy
            var attribute1 =
                Stats.GetStatIntByEnum(Equipment.Weapon.Skill.GetAttributeOfType<ItemParameter>().Attribute1)
                    .Current;
            var attribute2 =
                Stats.GetStatIntByEnum(Equipment.Weapon.Skill.GetAttributeOfType<ItemParameter>().Attribute2)
                    .Current;
            var passiveSkill = (int) PasiveSkills.GetSkillLevelByEnum(Equipment.Weapon.Skill)*5;
            //Turn to % value
            var weaponAccExtended = (1 + Equipment.Weapon.Stats.Accuracy.Extend/100);
            //TODO if weapon can lose its accuracy base and current algo required
            var accuracy = ((attribute1 + attribute2)*2 + passiveSkill + Level*3)*weaponAccExtended;
            Stats.Accuracy.Base = accuracy;
        }

        /// <summary>
        ///     Return experience required to promote
        /// </summary>
        /// <returns></returns>
        public double GetToNextLevelExperience()
        {
            return 40*Math.Pow(Level + 1, 3) + 1250;
        }

        /// <summary>
        ///     Try to promote player
        /// </summary>
        private void TryPromote()
        {
            var expected = GetToNextLevelExperience();
            if (Experience >= expected)
            {
                Experience = Experience - expected;
                Level++;
            }
        }

        /// <summary>
        ///     If character is created for the first time, set all bases stats.
        ///     TODO: Consider using this on level up
        /// </summary>
        public void SetAllBaseStats()
        {
            Stats.HealthPoints.Base = 10 + (Stats.Vitality.Base*5) + (Level*(1 + Stats.Vitality.Base/(float) 7.5));
            Stats.HealthRegen.Base = 0;

            Stats.ShieldPoints.Base = (1 + (Stats.Intelligence.Base + Stats.Wisdom.Base)/100);
            Stats.ShieldRegen.Base = Stats.ShieldPoints.Base/10;

            Stats.StaminaPoints.Base = 60 + (Stats.Vitality.Base*3) + (Level*(1 + Stats.Vitality.Base/25));
            Stats.StaminaRegen.Base = Stats.StaminaPoints.Base/5;

            Stats.ManaPoints.Base = 0 + (Stats.Intelligence.Base*2) + (Stats.Wisdom.Base*1) +
                                    (Level*(Stats.Intelligence.Base*2 + Stats.Wisdom.Base)/50);
            Stats.ManaRegen.Base = Stats.ManaPoints.Base/60;

            Stats.Evasion.Base = (Stats.Dexterity.Base + Stats.Perception.Base)*2 + Level*5;
        }
    }
}