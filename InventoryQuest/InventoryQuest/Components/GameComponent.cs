using System;
using System.Collections.Generic;

namespace InventoryQuest
{
    public abstract class GameComponent
    {
        /// <summary>
        ///     List of all alive game components.
        /// </summary>
        private static readonly List<GameComponent> allGameComponents = new List<GameComponent>();

        /// <summary>
        ///     List of all game components that will be removed at the end of the frame.
        /// </summary>
        private static readonly List<GameComponent> gameComponentsToRemove = new List<GameComponent>();

        protected GameComponent()
        {
            IsDestroyed = false;
            allGameComponents.Add(this);
            OnInit();
        }

        /// <summary>
        ///     Is this object already destroyed?
        /// </summary>
        public bool IsDestroyed { get; private set; }

        private void Update()
        {
            OnUpdate();
        }

        /// <summary>
        ///     This game component will be destroyed at the end of this frame.
        ///     What this means is, the object won't be actually destroyed, but Update function won't be called any more and
        ///     OnDestroy is called.
        /// </summary>
        public void Destroy()
        {
            if (IsDestroyed)
            {
                throw new InvalidOperationException("This item has been already destroyed");
            }

            OnDestroy();

            IsDestroyed = true;
            gameComponentsToRemove.Add(this);
        }

        /// <summary>
        ///     This method is called just after this GameComponent is created.
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        ///     This method is called every frame on update.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        ///     This method is called just before this object is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
        }

        // Static members region

        #region STATIC

        /// <summary>
        ///     Create new GameComponent and returns it.
        /// </summary>
        /// <typeparam name="int">GameComponent to create.</typeparam>
        internal static GameComponent AddComponent<T>() where T : GameComponent, new()
        {
            return new T();
        }

        /// <summary>
        ///     Update all alive GameCompontents.
        /// </summary>
        public static void UpdateAll()
        {
            for (var i = 0; i < allGameComponents.Count; i++)
            {
                allGameComponents[i].Update();
            }

            for (var i = 0; i < gameComponentsToRemove.Count; i++)
            {
                allGameComponents.Remove(gameComponentsToRemove[i]);
            }

            gameComponentsToRemove.Clear();
        }

        #endregion
    }
}