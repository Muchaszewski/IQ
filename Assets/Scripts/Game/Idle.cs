using System;
using System.Diagnostics;
using InventoryQuest.Components.Entities.Player;

namespace InventoryQuest.Game
{
    public class Idle : IIdle
    {
        public const float TravelLocaly = 30;
        public const float TravelToArea = 300;
        private readonly Player Player = CurrentGame.Instance.Player;
        public float InTravel { get; private set; }

        /// <summary>
        ///     Refill healt, mana, stamina
        /// </summary>
        /// <param name="all">
        ///     False - Ready to go if any of stats will be at given percent
        ///     <para>True - Wait for all stats to fill at least to given percent</para>
        /// </param>
        /// <param name="percent">Percent when player is ready to next battle</param>
        /// <returns></returns>
        public bool Refill(bool all, int percent = 100)
        {
            if (all)
            {
                return RefillAll(percent);
            }
            return RefillAny(percent);
        }

        /// <summary>
        ///     Refill healt, mana, stamina
        /// </summary>
        /// <param name="percent">Percent when player is ready to next battle</param>
        /// <returns></returns>
        public bool RefillAll(int percent = 100)
        {
            var firstSet = Math.Min(Player.Stats.HealthPoints.GetPercent(), Player.Stats.ManaPoints.GetPercent());
            if (Math.Abs(Player.Stats.ManaPoints.Base) < 0.03)
            {
                firstSet = Player.Stats.HealthPoints.GetPercent();
            }
            var secondSet = Math.Min(Player.Stats.StaminaPoints.GetPercent(), Player.Stats.ShieldPoints.GetPercent());
            if (Math.Abs(Player.Stats.ShieldPoints.Base) < 0.03)
            {
                secondSet = Player.Stats.StaminaPoints.GetPercent();
            }
            var minStatPercent = (int)Math.Min(firstSet, secondSet);
            if (percent > minStatPercent)
            {
                Player.Stats.HealthPoints.Regen(3, Player.Stats.HealthRegen);
                Player.Stats.ManaPoints.Regen(5, Player.Stats.ManaRegen);
                Player.Stats.StaminaPoints.Regen(0, Player.Stats.StaminaRegen);
                Player.Stats.ShieldPoints.Regen(0, Player.Stats.ShieldRegen);
                return false;
            }
            InvokeEvent_onRefilled(EventArgs.Empty);
            return true;
        }

        /// <summary>
        ///     Refill healt, mana, stamina
        /// </summary>
        /// <param name="percent">Percent when player is ready to next battle</param>
        /// <returns></returns>
        public bool RefillAny(int percent = 100)
        {
            var firstSet = Math.Max(Player.Stats.HealthPoints.GetPercent(), Player.Stats.ManaPoints.GetPercent());
            var secondSet = Math.Max(Player.Stats.StaminaPoints.GetPercent(), Player.Stats.ShieldPoints.GetPercent());
            var maxStatPercent = (int)Math.Max(firstSet, secondSet);
            if (percent > maxStatPercent)
            {
                Player.Stats.HealthPoints.Regen(3, Player.Stats.HealthRegen);
                Player.Stats.ManaPoints.Regen(5, Player.Stats.ManaRegen);
                Player.Stats.StaminaPoints.Regen(0, Player.Stats.StaminaRegen);
                Player.Stats.ShieldPoints.Regen(0, Player.Stats.ShieldRegen);
                return false;
            }
            InvokeEvent_onRefilled(EventArgs.Empty);
            return true;
        }

        /// <summary>
        ///     Event after refill is finished
        /// </summary>
        public static event EventHandler<EventArgs> onRefilled = delegate { };

        /// <summary>
        ///     Raise Event on Refilled
        /// </summary>
        protected void InvokeEvent_onRefilled(EventArgs args)
        {
            onRefilled.Invoke(this, args);
        }

        /// <summary>
        ///     Travel to another area/find monster
        /// </summary>
        /// <param name="travelLocaly">Is player travel to find monster</param>
        /// <param name="isFastTravel">TODO</param>
        /// <returns></returns>
        public bool Travel(bool travelLocaly, bool isFastTravel)
        {
            InTravel += Player.Stats.MovmentSpeed.Current + 1;
            if (travelLocaly)
            {
                if (InTravel >= TravelLocaly)
                {
                    InTravel = 0;
                    return true;
                }
            }
            else
            {
                if (InTravel >= TravelToArea)
                {
                    InTravel = 0;
                    return true;
                }
            }
            return false;
        }
    }
}