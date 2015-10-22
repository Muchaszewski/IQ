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
                    Player.Experience += experience;
                }

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
                        }
                        else
                        {
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
                if (!Move(Player, Target))
                {
                    //Debug.Log("Player " + Player.NextTurn);
                    if (Player.NextTurn <= 0)
                    {
                        Attack(Player, Target);
                        Player.NextTurn = TURN_TIME + Player.NextTurn;
                    }
                }
                foreach (Entity enemy in Enemy)
                {
                    enemy.NextTurn -= enemy.AttackSpeed * Time.deltaTime;
                    if (!Move(enemy, Player))
                    {
                        //Debug.Log("Enemy " + enemy.NextTurn);
                        if (enemy.NextTurn <= 0)
                        {
                            Attack(enemy, Player);
                            enemy.NextTurn = TURN_TIME + enemy.NextTurn;
                        }
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
                        target.Position -= target.Stats.MovmentSpeed.Current * Time.deltaTime;
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
                        target.Position += target.Stats.MovmentSpeed.Current * Time.deltaTime;
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

                var distanceToMove = entity.Stats.MovmentSpeed.Current * Time.deltaTime;
                var MinDistance = Math.Max(entity.Stats.Range.Extend, 0);

                if (entity.IsRightSide)
                {
                    if (entity.Position > entity.Stats.Range.Extend)
                    {
                        if (entity.Position - distanceToMove > MinDistance)
                        {
                            entity.Position -= distanceToMove;
                        }
                        else
                        {
                            entity.Position = MinDistance;
                        }
                        return true;
                    }
                }
                else
                {
                    if (entity.Position < -entity.Stats.Range.Extend)
                    {
                        if (entity.Position + distanceToMove < -MinDistance)
                        {
                            entity.Position += distanceToMove;
                        }
                        else
                        {
                            entity.Position = -MinDistance;
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
        public override void Attack(Entity me, Entity target)
        {
            AttackMessage message = new AttackMessage();

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
                    message.Add(EnumAttackMessage.Missed);
                    InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, message));
                    return;
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
                    message.Add(EnumAttackMessage.Parried);
                    InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, message));
                    return;
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
                    message.Add(EnumAttackMessage.Blocked);
                    InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, message));
                }
            }
            else
            {
                message.Add(EnumAttackMessage.Exhausted);
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
                if (critical > 0)
                {
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
                message.Add(EnumAttackMessage.Critical, critical);
                message.Add(EnumAttackMessage.FinalDamage, finalDamage);
                InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, message));
                return;
            }
            else
            {
                message.Add(EnumAttackMessage.Absorb);
                InvokeEvent_onAttack(new FightControllerEventArgs(this, me, target, message));
                return;
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
                Enemy.AddRange(RandomEnemyFactory.CreateNumberOfEnemies(CurrentGame.Instance.Spot, Random.Range(1, 4), EnumEntityRarity.Normal));
            }

            foreach (var entity in Enemy)
            {
                var maxRandom = Random.Range(-100, 100);
                var max = Mathf.Max(entity.Stats.Range.Extend + maxRandom, Player.Stats.Range.Extend + maxRandom);
                if (max < 0)
                {
                    entity.IsRightSide = false;
                }
                else
                {
                    entity.IsRightSide = true;
                }
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