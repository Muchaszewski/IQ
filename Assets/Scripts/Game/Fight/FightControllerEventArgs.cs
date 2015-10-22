using System;
using InventoryQuest.Components.Entities;

namespace InventoryQuest.Game.Fight
{
    public class FightControllerEventArgs : EventArgs
    {
        public FightControllerEventArgs(FightController fightController, Entity invoker, Entity target, FloatingMessage message)
        {
            FightController = fightController;
            Invoker = invoker;
            Target = target;
            Message = message;
        }
        public FightControllerEventArgs(FightController fightController, Entity invoker, Entity target) :
            this(fightController, invoker, target, new FloatingMessage())
        {
        }

        public FightControllerEventArgs(FightController fightController, Entity invoker) :
            this(fightController, invoker, null)
        {
        }

        public FightController FightController { get; set; }
        public Entity Invoker { get; set; }
        public Entity Target { get; set; }
        public FloatingMessage Message { get; set; }
    }
}