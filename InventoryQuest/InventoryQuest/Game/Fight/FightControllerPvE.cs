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

namespace InventoryQuest.Game.Fight
{
    public class FightControllerPvE : FightController
    {
        private readonly int TURN_TIME = 2;
        private StringBuilder _battleLog = new StringBuilder();
        private float _oneSecondTimer;

        /// <summary>
        ///     Constructor for new fight </para>
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
        public override Entity Target { get; protected set; }

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

            BattleLog.AppendLine();

            if (winner == Player)
            {
                // Experience
                foreach (Entity enemy in Enemy)
                {
                    double levelDelta = enemy.Level - Player.Level;
                    double experienceModifer = 0;
                    if (levelDelta < 0)
                    {
                        experienceModifer = Math.Max(1 - 0.005 * Math.Pow(levelDelta, 2), 0.05);
                    }
                    else if (levelDelta > 0)
                    {
                        experienceModifer = 1 + 0.005 * Math.Pow(levelDelta, 2);
                    }
                    else
                    {
                        // roznica_poziomu == 0
                        experienceModifer = 1;
                    }
                    var experience = 50 * experienceModifer;
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
                            (EnumItemRarity)RandomNumberGenerator.NextRandom(6));
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
            var isEnemyDead = false;
            foreach (Entity enemy in Enemy)
            {
                if (PreFight(enemy))
                {
                    isEnemyDead = true;
                }
            }
            if (isEnemyDead)
            {
                IsPlayerWinner = true;
                BattleLog.AppendLine("You have won!");

                BattleLog.AppendLine();
            }
            if (_oneSecondTimer >= 1)
            {
                _oneSecondTimer = 0;
            }
            //Fight
            if (IsFight)
            {
                Player.NextTurn -= Player.Stats.AttackSpeed.Extend * Time.deltaTime;
                if (Player.NextTurn <= 0)
                {
                    Target = null;
                    float targetValue = 0;
                    foreach (Entity enemy in Enemy)
                    {
                        if (enemy.Stats.HealthPoints.Extend > 0)
                        {
                            if (Target == null)
                            {
                                Target = enemy;
                                foreach (StatValueInt item in enemy.Stats.GetAllStatsInt())
                                {
                                    targetValue += item.Current * 10;
                                }
                            }
                            else
                            {
                                float newTargetValue = 0;
                                foreach (StatValueInt item in enemy.Stats.GetAllStatsInt())
                                {
                                    newTargetValue += item.Extend * 10;
                                }
                                newTargetValue += enemy.Stats.HealthPoints.Extend;
                                newTargetValue += enemy.Stats.ManaPoints.Extend;
                                if (newTargetValue < targetValue)
                                {
                                    Target = enemy;
                                    targetValue = newTargetValue;
                                }
                            }
                        }
                    }
                    if (Target == null)
                    {
                        Stop(null);
                        return;
                    }
                    Attack(Player, Target);
                    Player.NextTurn = TURN_TIME + Player.NextTurn;
                }
                foreach (Entity enemy in Enemy)
                {
                    enemy.NextTurn -= enemy.Stats.AttackSpeed.Extend * Time.deltaTime;
                    if (enemy.NextTurn <= 0)
                    {
                        Attack(enemy, Player);
                        enemy.NextTurn = TURN_TIME + enemy.NextTurn;
                    }
                }
            }
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
                entity.Stats.StaminaPoints.Regen(0, entity.Stats.StaminaPoints);
            }
            return false;
        }

        /// <summary>
        ///     Let entity attack
        /// </summary>
        /// <param name="me">Attacking entity</param>
        /// <param name="target">Target entity</param>
        private void Attack(Entity me, Entity target)
        {
            BattleLog.Append(me.Name + " attacked... ");

            //Use Stamina

            // TODO: get required strength for the weapon if attacker is the player
            var weaponsRequiredStrength = 0;
            var staminaUsed =
                10
                * (0.5f / me.Stats.AttackSpeed.Extend)
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
                        (((float)me.Stats.Accuracy.Extend / (me.Stats.Accuracy.Extend + target.Stats.Evasion.Extend)) *
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
                    return;
                }

                //Deflection
                var deflection =
                    (int)
                        (((float)me.Stats.Accuracy.Extend /
                          (me.Stats.Accuracy.Extend + target.Stats.Deflection.Extend)) * 100);
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
                    return;
                }

                //Block
                var block =
                    (int)
                        (((float)me.Stats.Accuracy.Extend /
                          (me.Stats.Accuracy.Extend + target.Stats.BlockChance.Extend)) * 100);
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
                }
            }
            else
            {
                BattleLog.AppendLine(" since " + target.Name + " is exhausted you ");
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
            }
            else
            {
                BattleLog.AppendLine("dealt no damage." +
                                     (absorbedDamage > 0 ? " (armor soaked " + absorbedDamage + ")" : ""));
            }
        }

        /// <summary>
        ///     Invoke if you want to do one side attack with no block/parry mechanics
        /// </summary>
        /// <param name="me">Who should make attack</param>
        public override void DoOneSideAttack(Entity me, Entity target)
        {
            //TODO
            int critical;
            float damage = me.Attack(out critical);
            damage -= target.Defend();
            target.Stats.HealthPoints.Current -= damage;
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
                Enemy.Add(RandomEnemyFactory.CreateEnemy(CurrentGame.Instance.Spot, EnumEntityRarity.Normal));
            }

            IsEnded = false;
            IsFight = true;
        }

        public void CountDownTimers()
        {
            _oneSecondTimer += Time.deltaTime / 1000f;
        }
    }
}