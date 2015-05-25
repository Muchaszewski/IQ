using System;

namespace InventoryQuest
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        AssertFail
    }

    public static class Logger
    {
        public static void Log(object message, LogLevel level = LogLevel.Info)
        {
            Log(message.ToString(), level);
        }

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            Console.WriteLine(message);
        }
    }
}