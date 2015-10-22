using System;
using System.Collections.Generic;
using System.Text;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Entities.Generation;
using InventoryQuest.Components.Entities.Player;

namespace InventoryQuest.Game.Fight
{
    /// <summary>
    ///     Base class for fight controller
    /// </summary>
    public abstract class FightController
    {
        private bool _IsPlayerWinner;
        //Is the battle in progres
        public bool IsFight { get; protected set; }
        //Is the battle ended
        public bool IsEnded { get; protected set; }
        //Is the player winner of this battle
        public bool IsPlayerWinner
        {
            get { return _IsPlayerWinner; }
            protected set
            {
                _IsPlayerWinner = value;
                if (_IsPlayerWinner)
                {
                    Stop(Player);
                }
                else
                {
                    Stop(null);
                }
            }
        }

        /// <summary>
        ///     Player
        /// </summary>
        public abstract Player Player { get; protected set; }

        /// <summary>
        ///     Entity
        /// </summary>
        public abstract List<Entity> Enemy { get; protected set; }

        /// <summary>
        ///     Current player target
        /// </summary>
        public abstract Entity Target { get; set; }

        /// <summary>
        ///     Event on Attack
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onAttack = delegate { };

        /// <summary>
        ///     Event on Defend
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onDefend = delegate { };

        /// <summary>
        ///     Event on Block
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onBlock = delegate { };

        /// <summary>
        ///     Event on Parry
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onParry = delegate { };

        /// <summary>
        ///     Event on Defeat
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onDefeat = delegate { };

        /// <summary>
        ///     Event on Victory
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onVicotry = delegate { };

        /// <summary>
        ///     Event on Melle Attack
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onMelleAttack = delegate { };

        /// <summary>
        ///     Event on Ranged Attack
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onRangedAttack = delegate { };

        /// <summary>
        ///     Event on move
        /// </summary>
        public static event EventHandler<FightControllerEventArgs> onMove = delegate { };

        public static event EventHandler<FightControllerEventArgs> onCreatingEnemies = delegate { };

        /// <summary>
        ///     Raise Event on Attack
        /// </summary>
        protected void InvokeEvent_onAttack(FightControllerEventArgs fightControllerEventArgs)
        {
            onAttack.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Defend
        /// </summary>
        protected void InvokeEvent_onDefend(FightControllerEventArgs fightControllerEventArgs)
        {
            onDefend.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Block
        /// </summary>
        protected void InvokeEvent_onBlock(FightControllerEventArgs fightControllerEventArgs)
        {
            onBlock.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Parry
        /// </summary>
        protected void InvokeEvent_onParry(FightControllerEventArgs fightControllerEventArgs)
        {
            onParry.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Defeat
        /// </summary>
        protected void InvokeEvent_onDefeat(FightControllerEventArgs fightControllerEventArgs)
        {
            onDefeat.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Vicotry
        /// </summary>
        protected void InvokeEvent_onVicotry(FightControllerEventArgs fightControllerEventArgs)
        {
            onVicotry.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Melle Attack
        /// </summary>
        protected void InvokeEvent_onMelleAttack(FightControllerEventArgs fightControllerEventArgs)
        {
            onMelleAttack.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Ranged Attack
        /// </summary>
        protected void InvokeEvent_onRangedAttack(FightControllerEventArgs fightControllerEventArgs)
        {
            onRangedAttack.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     Raise Event on Move
        /// </summary>
        /// <param name="fightControllerEventArgs"></param>
        protected void InvokeEvent_onMove(FightControllerEventArgs fightControllerEventArgs)
        {
            onMove.Invoke(this, fightControllerEventArgs);
        }


        protected void InvokeEvent_onCreatingEnemies(FightControllerEventArgs fightControllerEventArgs)
        {
            onCreatingEnemies.Invoke(this, fightControllerEventArgs);
        }

        /// <summary>
        ///     StartLoop battle
        ///     TODO change to generic
        /// </summary>
        public abstract FightControllerPvE Begin();

        /// <summary>
        ///     Pause battle
        /// </summary>
        public abstract void Pause();

        /// <summary>
        ///     Resume paused battle
        /// </summary>
        public abstract void Resume();

        /// <summary>
        ///     Force to stop battle with Draw
        /// </summary>
        public abstract void ForceStop();

        /// <summary>
        ///     Force to stop battle with winner
        /// </summary>
        /// <param name="winner">Winner</param>
        public abstract void Stop(Entity winner);

        /// <summary>
        ///     Skip battle to end
        /// </summary>
        public abstract void FastForward();

        /// <summary>
        ///     Invoke every step battle, attacking/defending logic
        /// </summary>
        public abstract void DoFight();

        /// <summary>
        ///     TODO? Create new enemy
        /// </summary>
        public virtual void ResetBattle()
        {
            Enemy = new List<Entity>();
            Enemy.Add(RandomEnemyFactory.CreateEnemy(CurrentGame.Instance.Spot, EnumEntityRarity.Normal));
            IsEnded = false;
            IsFight = true;
        }

        public abstract void Attack(Entity me, Entity target);
    }
}