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
using InventoryQuest.InventoryQuest.Components.ActionEvents;

namespace InventoryQuest.Game
{
    [Serializable]
    public class CurrentGame : MonoBehaviour
    {
        private int _travelToSpot = -1;
        private byte Refiling;
        private byte Traveling;
        public Player Player { get; set; }
        public AudioClip PlayerUnarmedAudioClipHit;
        public AudioClip PlayerUnarmedAudioClipParry;

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
            //Invoke new charater creation
            //TODO move to proper menu
            ActionEventManager.Misc.OnNewCharacter_Invoke();

            instance = this;
            Spot = GenerationStorage.Instance.Spots[0];
            Spot.IsUnlocked = true;

            var stats = new Stats();
            stats.Strength.Base = UnityEngine.Random.Range(0, 20);
            Player = new Player("Unnamed", 1, stats);
            Player.IsAlive = true;
            Idle = new Idle();
            FightController = new FightControllerPvE(Player).Begin();
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
                            ActionEventManager.Fight.OnTravelEnd_Invoke();
                        }
                        Traveling = 2;
                        FightController.ResetBattle();
                        ActionEventManager.Fight.OnEnemiesFound_Invoke();
                        ResetIdle();
                        return;
                    }
                    if (Traveling == 0)
                    {
                        Traveling = 1;
                        if (TravelToSpot != -1)
                        {
                            ActionEventManager.Fight.OnTravelBegin_Invoke();
                        }
                        else
                        {
                            ActionEventManager.Fight.OnLookingForEnemies_Invoke();
                        }
                    }
                }
                //If character is healty enough
                if (Idle.Refill(true))
                {
                    Refiling = 2;
                    ActionEventManager.Regen.HealthRegen.OnEnd_Invoke();
                    ActionEventManager.Regen.ManaRegen.OnEnd_Invoke();
                    ActionEventManager.Regen.StaminaRegen.OnEnd_Invoke();
                }
                else if (Refiling == 0)
                {
                    ActionEventManager.Regen.HealthRegen.OnBegin_Invoke();
                    ActionEventManager.Regen.ManaRegen.OnBegin_Invoke();
                    ActionEventManager.Regen.StaminaRegen.OnBegin_Invoke();
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
            ActionEventManager.Misc.OnLoad_Invoke();
        }


        public void Save()
        {
            BinaryFilesOperations.Save(Instance.Player, "SaveFile.sav");
            ActionEventManager.Misc.OnSave_Invoke();
        }

        public void InvokeChangeSpot()
        {
            ActionEventManager.Fight.OnTravelPlanned_Invoke();
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