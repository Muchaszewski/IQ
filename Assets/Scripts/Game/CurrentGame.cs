using System.Collections.Generic;
using InventoryQuest.Components;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Entities.Generation;
using InventoryQuest.Components.Entities.Player;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game.Fight;
using InventoryQuest.Utils;
using UnityEngine;
using System;
using System.Collections;

namespace InventoryQuest.Game
{
    [Serializable]
    public class CurrentGame : MonoBehaviour
    {
        public static event EventHandler TravelingFinished = delegate { };

        private int _travelToSpot = -1;
        private byte Refiling;
        private byte Traveling;
        public Player Player { get; set; }
        public FightController FightController { get; set; }
        public Idle Idle { get; set; }

        public int TravelToSpot
        {
            get { return _travelToSpot; }
            set { _travelToSpot = value; }
        }

        public Spot Spot { get; set; }

        /// <summary>
        ///     Waiting for CurrentGame Instance to create
        /// </summary>
        private void Awake()
        {
            instance = this;
            Spot = GenerationStorage.Instance.Spots[0];

            var stats = new Stats();
            stats.Strength.Base = UnityEngine.Random.Range(0, 20);
            Player = new Player("Unnamed", 1, stats);
            var Enemy = new List<Entity> { RandomEnemyFactory.CreateEnemy(Spot, EnumEntityRarity.Normal) };
            Idle = new Idle();
            FightController = new FightControllerPvE(Player, Enemy).Begin();
            FightController.Pause();
        }

        private void Update()
        {
            //If fight is in progress
            if (FightController.IsFight)
            {
                FightController.DoFight();
            }
            //If fight has ended
            if (FightController.IsEnded)
            {
                if (Refiling == 2)
                {
                    //If treveling has finished
                    if (Idle.Travel(TravelToSpot == -1, false))
                    {
                        if (TravelToSpot != -1)
                        {
                            Spot = GenerationStorage.Instance.Spots[TravelToSpot];
                            TravelToSpot = -1;
                            ((FightControllerPvE)FightController).FightsInCurrentSpot = 0;
                        }
                        Traveling = 2;
                        FightController.ResetBattle();
                        ResetIdle();
                        TravelingFinished.Invoke(this, EventArgs.Empty);
                        return;
                    }
                    if (Traveling == 0)
                    {
                        Traveling = 1;
                        if (TravelToSpot != -1)
                        {
                            FightController.BattleLog.AppendLine("----------------------------------------");
                            FightController.BattleLog.AppendLine("Traveling to: " +
                                                                 GenerationStorage.Instance.Spots[TravelToSpot].Name);
                            FightController.BattleLog.AppendLine("----------------------------------------");
                        }
                        else
                        {
                            FightController.BattleLog.AppendLine("----------------------------------------");
                            FightController.BattleLog.AppendLine("Looking for enemies...");
                            FightController.BattleLog.AppendLine("----------------------------------------");
                        }
                    }
                }
                //If character is healty enough
                if (Idle.Refill(true))
                {
                    Refiling = 2;
                }
                else if (Refiling == 0)
                {
                    FightController.BattleLog.AppendLine("Reffiling");
                    Refiling = 1;
                }
            }
        }

        private void ResetIdle()
        {
            Refiling = 0;
            Traveling = 0;
        }

        public void Load()
        {
            Player = BinaryFilesOperations.Load<Player>("SaveFile.sav");
            InventoryPanel.Instance.PopulateInventory();
            foreach (var item in FindObjectsOfType<StatisticHandler>())
            {
                item.RecalculateStatistics();
            }
        }


        public void Save()
        {
            BinaryFilesOperations.Save(Instance.Player, "SaveFile.sav");
        }

        #region static

        private static CurrentGame instance;

        public static CurrentGame Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}