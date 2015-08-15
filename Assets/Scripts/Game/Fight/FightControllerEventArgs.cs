using System;
using InventoryQuest.Components.Entities;

namespace InventoryQuest.Game.Fight
{
    public class FightControllerEventArgs : EventArgs
    {
        public FightControllerEventArgs(FightController fightController, Entity invoker, Entity target)
        {
            FightController = fightController;
            Invoker = invoker;
            Target = target;
        }

        public FightControllerEventArgs(FightController fightController, Entity invoker) :
            this(fightController, invoker, null)
        {
        }

        public FightController FightController { get; set; }
        public Entity Invoker { get; set; }
        public Entity Target { get; set; }
    }
}