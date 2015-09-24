using System;
using System.Collections.Generic;
using System.Text;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Entities.Generation;
using InventoryQuest.Components.Entities.Player;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text.RegularExpressions;
using System.Linq;

namespace InventoryQuest.Game.Fight
{
    public class FightControllerPvE : FightController
    {
        private readonly int TURN_TIME = 2;
        private StringBuilder _battleLog = new StringBuilder();
        private float _oneSecondTimer;

        /// <summary>
        ///     Constructor for new fight
        ///     Fight not begin yet
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="enemy">Enemy</param>
        public FightControllerPvE(Entity player, List<Entity> enemy)
        {
            FightsInCurrentSpot = 0;
            Player = (Player)player;
            Enemy = enemy;
        }

        /// <summary>
        ///     Battle log generated while fight
        /// </summary>
        public override StringBuilder BattleLog
        {
            get { return _battleLog; }
            set { _battleLog = value; }
        }

        /// <summary>
        ///     Fighting player
        /// </summary>
        public override Player Player { get; protected set; }

        /// <summary>
        ///     Fighting enemy
        /// </summary>
        public override List<Entity> Enemy { get; protected set; }

        /// <summary>
        ///     Player target
        /// </summary>
        public override Entity Target { get; set; }

        public int FightsInCurrentSpot { get; set; }

        /// <summary>
        ///     StartLoop battle
        /// </summary>
        public override FightControllerPvE Begin()
        {
            if (Enemy == null || Enemy.Count == 0)
            {
                IsFight = false;
                IsEnded = true;
                BattleLog.AppendLine("No enemies to fight with!");
                return this;
            }
            IsFight = true;
            InvokeEvent_onCreatingEnemies(new FightControllerEventArgs(this, null));
            return this;
        }

        /// <summary>
        ///     Pause battle
        /// </summary>
        public override void Pause()
        {
            IsFight = false;
        }

        /// <summary>
        ///     Resume paused battle
        /// </summary>
        public override void Resume()
        {
            Begin();
        }

        /// <summary>
        ///     Force to stop battle with Draw
        /// </summary>
        public override void ForceStop()
        {
            IsFight = false;
            IsEnded = true;
        }

        /// <summary>
        ///     Force to stop battle with winner
        /// </summary>
        /// <param name="loser">Loser</param>
        public override void Stop(Entity winner)
        {
            IsFight = false;
            IsEnded = true;

            if (BattleLog.Length > 1000)
            {
                var lines = Regex.Split(BattleLog.ToString(), "\r\n|\r|\n").Skip(20);
                BattleLog = new StringBuilder();
                BattleLog.Insert(0, string.Join(Environment.NewLine, lines.ToArray()));
            }

            BattleLog.AppendLine();

            if (winner == Player)
            {
                InvokeEvent_onVicotry(new FightControllerEventArgs(this, Player, null));
                // Experience
                foreach (Entity enemy in Enemy)
                {
                    double levelDelta = enemy.Level - Player.Level;
                    double experienceModifer = 0;
                    if (levelDelta < 0)
                    {
                        experienceModifer = Math.Max(1 - 0.005*Math.Pow(levelDelta, 2), 0.05);
                    }
                    else if (levelDelta > 0)
                    {
                        experienceModifer = 1 + 0.005*Math.Pow(levelDelta, 2);
                    }
                    else
                    {
                        // roznica_poziomu == 0
                        experienceModifer = 1;
                    }
                    var experience = 50*experienceModifer;
                    BattleLog.AppendLine("Gained " + experience + " experience");
                    Player.Experience += experience;
                }
                BattleLog.AppendLine("Experience required for the next level: " +
                                     (Player.GetToNextLevelExperience() - Player.Experience));

                // Drop items
                var dropRolls = 2;
                var dropChance = 10;


                for (var i = 0; i < dropRolls; i++)
                {
                    if (RandomNumberGenerator.NextRandom(100) <= dropChance)
                    {
                        Item item = RandomItemFactory.CreateItem(
                            CurrentGame.Instance.Spot,
                            (EnumItemRarity) RandomNumberGenerator.NextRandom(6));
                        if (item != null)
                        {
                            Player.Inventory.AddItem(item);
                            BattleLog.AppendLine("You have found: " + item.Name);
                        }
                        else
                        {
                            BattleLog.AppendLine("Error: Item was null!");
                        }
                    }
                }
            }
            else
            {
                InvokeEvent_onDefeat(new FightControllerEventArgs(this, Player, null));
            }

        }

        /// <summary>
        ///     Skip battle to end
        /// </summary>
        public override void FastForward()
        {
            while (IsFight && !IsEnded)
            {
                DoFight();
            }
        }

        /// <summary>
        ///     Invoke every step battle, attacking/defending logic
        /// </summary>
        public override void DoFight()
        {
            //Count down every timer
            CountDownTimers();
            //Prefight
            if (PreFight(Player))
            {
                IsPlayerWinner = false;
                BattleLog.AppendLine("You have lost...");
                BattleLog.AppendLine();
            }
            var areEnemiesDead = true;
            foreach (Entity enemy in Enemy)
            {
                if (!PreFight(enemy))
                {
                    areEnemiesDead = false;
                }
            }
            if (areEnemiesDead)
            {
                IsPlayerWinner = true;
                BattleLog.AppendLine("You have won!");

                BattleLog.AppendLine();
            }
            //Reset regen timer
            if (_oneSecondTimer >= 1)
            {
                _oneSecondTimer = 0;
            }
            //Fight
            if (IsFight)
            {
                Player.NextTurn -= Player.AttackSpeed * Time.deltaTime;
                if (Target == null || Target.Stats.HealthPoints.Current <= 0)
                {
                    Target = null;
                    float targetValue = 0;
                    foreach (Entity enemy in Enemy)
                    {
                        if (enemy.Stats.HealthPoints.Current > 0)
                        {
                            if (Target == null)
                            {
                                targetValue = enemy.Position;
                                Target = enemy;
                            }
                            else
                            {
                                if (targetValue > enemy.Position)
                                {
                                    Target = enemy;
                                }
                            }
                        }
                    }
                    if (Target == null)
                    {
                        Stop(null);
                        return;
                    }
                }
                if (Player.NextTurn <= 0)
                {
                    if (!Move(Player, Target))
                    {
                        Attack(Player, Target);
                    }
                    Player.NextTurn = TURN_TIME + Player.NextTurn;
                }
                foreach (Entity enemy in Enemy)
                {
                    enemy.NextTurn -= enemy.AttackSpeed * Time.deltaTime;
                    if (enemy.NextTurn <= 0)
                    {
                        if (!Move(enemy, Player))
                        {
                            Attack(enemy, Player);
                        }
                        enemy.NextTurn = TURN_TIME + enemy.NextTurn;
                    }
                }
            }
        }

        private bool Move(Entity entity, Entity target)
        {
            if (entity.Type == EnumEntityType.Player)
            {
                if (target.Position > 0)
                {
                    if (target.Position > entity.Stats.Range.Extend)
                    {
                        target.Position -= target.Stats.MovmentSpeed.Current;
                        if (target.Position < 0)
                        {
                            target.Position = 0;
                        }
                        return true;
                    }
                }
                else if (target.Position < 0)
                {
                    if (target.Position < -entity.Stats.Range.Extend)
                    {
                        target.Position += target.Stats.MovmentSpeed.Current;
                        if (target.Position > 0)
                        {
                            target.Position = -1f;
                        }
                        return true;
                    }
                }
            }
            else
            {
                if (entity.Position > 0)
                {
                    if (entity.Position > entity.Stats.Range.Extend)
                    {
                        entity.Position -= entity.Stats.MovmentSpeed.Current;
                        if (entity.Position < 0)
                        {
                            entity.Position = 0;
                        }
                        return true;
                    }
                }
                else if (entity.Position < 0)
                {
                    if (entity.Position < -entity.Stats.Range.Extend)
                    {
                        entity.Position += entity.Stats.MovmentSpeed.Current;
                        if (target.Position > 0)
                        {
                            target.Position = -1f;
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Checking player/enemy statsu.
        /// </summary>
        private bool PreFight(Entity entity)
        {
            if (entity.Stats.StaminaPoints.Current <= -1000 * entity.Stats.StaminaPoints.Extend)
            {
                return entity.Stats.HealthPoints.Current <= 0;
            }
            if (entity.Stats.HealthPoints.Current <= 0)
            {
                return entity.Stats.HealthPoints.Current <= 0;
            }
            //If not dead
            if (entity.NoDamageTime >= 5)
            {
                entity.Stats.ShieldPoints.Regen(0, entity.Stats.ShieldRegen);
            }
            if (_oneSecondTimer >= 1)
            {
                entity.Stats.HealthPoints.Regen(0, entity.Stats.HealthRegen);
                entity.Stats.ManaPoints.Regen(0, entity.Stats.ManaRegen);
                entity.Stats.StaminaPoints.Regen(0, entity.Stats.StaminaRegen);
            }
            return false;
        }

        /// <summary>
        ///     Let entity attack
        /// </summary>
        /// <param name="me">Attacking entity</param>
        /// <param name="target">Target entity</param>
        public override string Attack(Entity me, Entity target)
        {
            string returnMessage = "";
            BattleLog.Append(me.Name + " attacked... ");

            //Use Stamina
            var weaponsRequiredStrength = 0;
            if (me.Type == EnumEntityType.Player && ((Player)me).Equipment.Weapon != null)
            {
                weaponsRequiredStrength = ((Player)me).Equipment.Weapon.RequiredStats.Find(x => x.Type == EnumTypeStat.Strength).Extend;
            }
            var staminaUsed =
                10
                * (0.5f / me.AttackSpeed)
                * Math.Max((100 - me.Stats.Strength.Extend + weaponsRequiredStrength) / 100f, 0.5f);
            if (float.IsInfinity(staminaUsed))
            {
                Debug.LogWarning("Current stamina usage was Infinity, posible deviding by 0?");
                staminaUsed = 0;
            }
            me.Stats.StaminaPoints.Current -= staminaUsed;

            var blockAmount = 0;

            if (target.Stats.StaminaPoints.Extend > -100 * target.Stats.StaminaPoints.Extend)
            {
                //Evasion
                var evasion =
                    (int)
                        (((float)me.Accuracy / (me.Accuracy + target.Stats.Evasion.Extend)) *
                         100);
                if (evasion > 95)
                {
                    evasion = 95;
                }
                if (evasion < 5)
                {
                    evasion = 5;
                }
                if (RandomNumberGenerator.BoolRandom(evasion))
                {
                    BattleLog.AppendLine("but missed.");
                    InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, "/m" + "missed"));
                    return "/m" + "missed";
                }

                //Deflection
                var deflection =
                    (int)
                        (((float)me.Accuracy /
                          (me.Accuracy + target.Parry)) * 100);
                if (deflection > 95)
                {
                    deflection = 95;
                }
                if (deflection < 5)
                {
                    deflection = 5;
                }
                if (RandomNumberGenerator.BoolRandom(deflection))
                {
                    BattleLog.AppendLine("but " + target.Name + " parried.");
                    InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, "/p" + "parried"));
                    return "/p" + "parried";
                }

                //Block
                var block =
                    (int)
                        (((float)me.Accuracy /
                          (me.Accuracy + target.Stats.BlockChance.Extend)) * 100);
                if (block > 95)
                {
                    block = 95;
                }
                if (block < 5)
                {
                    block = 5;
                }
                if (RandomNumberGenerator.BoolRandom(block))
                {
                    blockAmount = target.Stats.BlockAmount.Extend;
                    BattleLog.AppendLine(target.Name + " blocked " + block + " damage, ");
                    InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, "/b" + "blocked" + "@"));
                    returnMessage += "/b" + "blocked" + "@";
                }
            }
            else
            {
                BattleLog.AppendLine(" since " + target.Name + " is exhausted you ");
                returnMessage += "/e" + "exhausted" + "@";
            }

            //Attack
            int critical;
            float damage = me.Attack(out critical);

            //Reduce damage by blockAmount
            damage -= blockAmount;

            //Defend and pierce
            var pierce = me.Pierce();
            float armor = target.Defend();

            // Modify armor value by attack's pierce value
            armor *= (1 - (pierce / 100f));

            // Reduce damage by armor
            var damageReduction = armor / (armor + damage);

            //Final damage
            var finalDamage = damage * (1 - damageReduction);
            if (finalDamage < 0)
                finalDamage = 0;
            var absorbedDamage = damage - finalDamage;

            if (finalDamage > 0)
            {
                BattleLog.AppendLine(finalDamage + " damage!" +
                                     (absorbedDamage > 0 ? " (armor soaked " + absorbedDamage + ")" : ""));
                if (critical > 0)
                {
                    BattleLog.AppendLine("Additional damage form critical strike: +" + me.Stats.CriticalDamage + "%" +
                                         " (" + critical + ")");
                }
                if (target.Stats.ShieldPoints.Current > 0)
                {
                    target.Stats.ShieldPoints.Current -= finalDamage;
                    if (target.Stats.ShieldPoints.Current < 0)
                    {
                        finalDamage = -target.Stats.ShieldPoints.Current;
                        target.Stats.ShieldPoints.Current = 0;
                    }
                }
                if (finalDamage > 0)
                {
                    target.Stats.HealthPoints.Current -= finalDamage;
                }
                InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, returnMessage + "/c" + critical + "@" + finalDamage));
                return returnMessage + "/c" + critical + "@" + finalDamage;
            }
            else
            {
                BattleLog.AppendLine("dealt no damage." +
                                     (absorbedDamage > 0 ? " (armor soaked " + absorbedDamage + ")" : ""));
                returnMessage += "/a" + "@";
                InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, returnMessage + "0"));
                return returnMessage + "0";
            }
        }

        public override void ResetBattle()
        {
            FightsInCurrentSpot++;
            Enemy = new List<Entity>();
            if (FightsInCurrentSpot % 500 == 0)
            {
                Enemy.Add(RandomEnemyFactory.CreateEnemy(CurrentGame.Instance.Spot, EnumEntityRarity.Unique));
            }
            else if (FightsInCurrentSpot % 100 == 0)
            {
                Enemy.Add(RandomEnemyFactory.CreateEnemy(CurrentGame.Instance.Spot, EnumEntityRarity.Rare));
            }
            else if (FightsInCurrentSpot % 20 == 0)
            {
                Enemy.Add(RandomEnemyFactory.CreateEnemy(CurrentGame.Instance.Spot, EnumEntityRarity.Uncommon));
            }
            else
            {
                Enemy.AddRange(RandomEnemyFactory.CreateNumberOfEnemies(CurrentGame.Instance.Spot, CurrentGame.Instance.Player.Level, Random.Range(1, 4), EnumEntityRarity.Normal));
            }

            foreach (var entity in Enemy)
            {
                var max = Mathf.Max(entity.Stats.Range.Extend, Player.Stats.Range.Extend);
                max = Random.Range(-50, 50);
                entity.Position = max;
            }

            IsEnded = false;
            IsFight = true;

            InvokeEvent_onCreatingEnemies(new FightControllerEventArgs(this, null));
        }

        public void CountDownTimers()
        {
            _oneSecondTimer += Time.deltaTime;
        }
    }
}