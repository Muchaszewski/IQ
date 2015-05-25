using System;

namespace InventoryQuest
{
    public static class Utilities
    {
        /// <summary>
        ///     Checks assertion. If condition == false, throws an Exception.
        /// </summary>
        /// <param name="condition">If this is false, assertion will fail</param>
        /// <param name="info">Info about assert</param>
        public static void Assert(bool condition, string info)
        {
            if (condition)
            {
                return;
            }

            Logger.Log("Assertion failed: " + info, LogLevel.Error);
            throw new Exception("Assertion failed: " + info);
        }
    }
}